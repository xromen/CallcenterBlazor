using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Callcenter.Api.Extensions;

public static class EntityFrameworkExtensions
{
    public static IQueryable<T> FromSqlWithDtoAsync<T>(
        this DbSet<T> dbSet, string sql, object dto) where T : class
    {
        var parameters = dto.GetType()
            .GetProperties()
            .Select(p => new NpgsqlParameter("@" + p.Name, p.GetValue(dto) ?? DBNull.Value))
            .ToArray();

        return dbSet.FromSqlRaw(sql, parameters);
    }
}