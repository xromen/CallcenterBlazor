using System.Linq.Expressions;
using Callcenter.Shared.Requests;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Callcenter.Api.Extensions;

public static class EntityFrameworkExtensions
{
    private static object? ChangeType(object? value, Type targetType)
    {
        if (value == null) return null;

        if (targetType == typeof(DateOnly))
        {
            if (value is string s && DateOnly.TryParse(s.Substring(0, s.Contains('T') ? s.IndexOf('T') : s.Length), out var result))
                return result;
            throw new InvalidCastException($"Невозможно преобразовать '{value}' в DateOnly");
        }

        return Convert.ChangeType(value, targetType);
    }
    
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> queryable, IEnumerable<FilterRequestDto> filters) where T : class
    {
        foreach (var filter in filters)
        {
            if (string.IsNullOrEmpty(filter.Field))
                continue;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, filter.Field);

            var targetType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;

            object? convertedValue = null;
            if (filter.Value != null)
            {
                var jsonString = filter.Value.ToString();
                convertedValue = ChangeType(jsonString, targetType);
            }

            var valueConstant = Expression.Constant(convertedValue, property.Type);

            Expression body = null;

            switch (filter.Operator?.ToLowerInvariant())
            {
                // ===== String =====
                case "contains":
                {
                    var constant = Expression.Constant($"%{convertedValue}%");
                    body = MyExpressions.ILike(property, constant);
                    break;
                }
                case "not contains":
                {
                    var constant = Expression.Constant($"%{convertedValue}%");
                    body = Expression.Not(MyExpressions.ILike(property, constant));
                    break;
                }
                case "equals":
                case "=":
                case "is":
                    if (convertedValue.GetType() == typeof(string))
                    {
                        body = MyExpressions.ILike(property, valueConstant);
                    }
                    else
                    {
                        body = Expression.Equal(property, valueConstant);
                    }
                    break;
                case "not equals":
                case "!=":
                case "is not":
                    if (convertedValue.GetType() == typeof(string))
                    {
                        body = Expression.Not(MyExpressions.ILike(property, valueConstant));
                    }
                    else
                    {
                        body = Expression.NotEqual(property, valueConstant);
                    }
                    break;
                case "starts with":
                {
                    var constant = Expression.Constant($"{convertedValue}%");
                    body = MyExpressions.ILike(property, constant);
                    break;
                }
                case "ends with":
                {
                    var constant = Expression.Constant($"%{convertedValue}");
                    body = MyExpressions.ILike(property, constant);
                    break;
                }
                case "is empty":
                    body = Expression.Equal(property, Expression.Constant(null, property.Type));
                    break;
                case "is not empty":
                    body = Expression.NotEqual(property, Expression.Constant(null, property.Type));
                    break;

                // ===== Number =====
                case ">":
                    body = Expression.GreaterThan(property, valueConstant);
                    break;
                case ">=":
                    body = Expression.GreaterThanOrEqual(property, valueConstant);
                    break;
                case "<":
                    body = Expression.LessThan(property, valueConstant);
                    break;
                case "<=":
                    body = Expression.LessThanOrEqual(property, valueConstant);
                    break;

                // ===== DateTime / DateOnly =====
                case "is after":
                    body = Expression.GreaterThan(property, valueConstant);
                    break;
                case "is on or after":
                    body = Expression.GreaterThanOrEqual(property, valueConstant);
                    break;
                case "is before":
                    body = Expression.LessThan(property, valueConstant);
                    break;
                case "is on or before":
                    body = Expression.LessThanOrEqual(property, valueConstant);
                    break;
            }

            if (body != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
                queryable = queryable.Where(lambda);
            }
        }

        return queryable;
    }

    public static IQueryable<T> ApplyOrders<T>(this IQueryable<T> queryable, IEnumerable<OrderRequestDto> orders)
    {
        foreach (var order in orders)
        {
            var dir = order.Descending ? "desc" : "asc";
            queryable = queryable.OrderBy($"{order.Field} {dir}");
        }
        
        return queryable;
    }
}

public class MyExpressions
{
    public static Expression ILike(Expression property, Expression value)
    {
        return Expression.Call(
            typeof(NpgsqlDbFunctionsExtensions),
            nameof(NpgsqlDbFunctionsExtensions.ILike),
            Type.EmptyTypes,
            Expression.Property(null, typeof(EF),
                nameof(EF.Functions)),
            property,
            value);
    }
}