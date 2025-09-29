namespace Callcenter.Api.Models.Excel;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ExcelColumnAttribute : Attribute
{
    public string ColumnName { get; set; }
    
    public string Format { get; set; }
}