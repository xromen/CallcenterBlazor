using System.Reflection;
using Callcenter.Api.Models.Excel;

namespace Callcenter.Api.Extensions;

public static class ReflectionExtensions
{
    private static Dictionary<Type, PropertyInfo[]> _propertiesCache = new ();
    private static Dictionary<PropertyInfo, ExcelColumnAttribute?> _propertyColumnAttributeCache = new ();
    
    public static PropertyInfo[] GetCachedProperties(this Type type)
    {
        if (!_propertiesCache.ContainsKey(type))
        {
            _propertiesCache.Add(type, type.GetProperties());
        }
        
        return _propertiesCache[type];
    }

    public static ExcelColumnAttribute GetCachedColumnAttribute(this PropertyInfo property)
    {
        if (!_propertyColumnAttributeCache.ContainsKey(property))
        {
            var attribute = property.GetCustomAttribute(typeof(ExcelColumnAttribute), true) as ExcelColumnAttribute;
            
            _propertyColumnAttributeCache.Add(property, attribute);
        }
        
        return _propertyColumnAttributeCache.GetValueOrDefault(property) ?? new();
    }
}