using OfficeOpenXml;

namespace Callcenter.Api.Extensions;

public static class ExcelExtensions
{
    public static void SetDateValue(this ExcelRange cell, DateTime date)
    {
        cell.Value = date;
        cell.Style.Numberformat.Format = "dd.MM.yyyy";
    }
    
    public static void SetDateValue(this ExcelRange cell, DateOnly date)
    {
        cell.SetDateValue(date.ToDateTime(TimeOnly.MinValue));
    }
}