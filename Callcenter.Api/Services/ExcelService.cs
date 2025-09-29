using Callcenter.Api.Extensions;
using OfficeOpenXml;

namespace Callcenter.Api.Services;

public class ExcelService
{
    public Task<byte[]> ExportAsync<T>(
        IEnumerable<T> items,
        CancellationToken cancellationToken = default)
        {
            return Task.Run<byte[]>(() =>
            {
                var properties = typeof(T).GetCachedProperties();
                ExcelPackage package = new ExcelPackage();
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Выгрузка");

                int row = 1;
                int col = 1;
                //Шапка
                foreach (var property in properties)
                {
                    var attribute = property.GetCachedColumnAttribute();

                    worksheet.Cells[row, col].Value = attribute.ColumnName ?? property.Name;

                    col++;
                }

                col = 1;
                row = 2;
                //Данные
                foreach (var item in items)
                {
                    foreach (var property in properties)
                    {
                        var attribute = property.GetCachedColumnAttribute();
                        var value = property.GetValue(item);

                        if (value is string str)
                        {
                            worksheet.Cells[row, col].Value = str.Trim();
                        }
                        else
                        {
                            worksheet.Cells[row, col].Value = property.GetValue(item);
                        }

                        if (!string.IsNullOrWhiteSpace(attribute.Format))
                        {
                            worksheet.Cells[row, col].Style.Numberformat.Format = attribute.Format;
                        }

                        col++;
                    }

                    row++;
                    col = 1;
                }

                foreach (var column in worksheet.Columns)
                {
                    column.AutoFit();
                }

                return package.GetAsByteArray();
            }, cancellationToken);
        }
}