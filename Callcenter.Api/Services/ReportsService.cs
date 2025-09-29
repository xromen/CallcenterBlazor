using System.Drawing;
using Callcenter.Api.Data;
using Callcenter.Api.Extensions;
using Callcenter.Api.Models;
using Callcenter.Api.Models.Exceptions;
using Callcenter.Api.Models.Reports;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Callcenter.Api.Services;

public class ReportsService(
    ApplicationDbContext dbContext,
    IWebHostEnvironment env,
    RequestEnvironment requestEnvironment)
{
    public async Task<byte[]> GetPgFormGeneral(DateOnly from, DateOnly to, CancellationToken cancellationToken)
    {
        if (requestEnvironment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }

        var data = await dbContext.Database
            .SqlQueryRaw<PgFormGeneralModel>("select * from sp_pg_new_report({0}, {1}, {2})",
                requestEnvironment.AuthUser.Id, from, to)
            .ToListAsync(cancellationToken);

        var templatePath = Path.Combine(env.ContentRootPath, "Templates", "PG_new_new.xlsx");
        ExcelPackage package = new ExcelPackage(templatePath);
        ExcelWorksheet sheet = package.Workbook.Worksheets.First();

        //Просто не спрашивай, пожалуйста...
        DateTime dateRazdelenia = new DateTime(2024, 9, 5);

        for (int row = 9; row <= 87; row++)
        {
            var rowData = GetPgGeneralRowData(data,
                row,
                sheet.Cells[row, 11].Value.ToString(),
                sheet.Cells[row, 2].Value.ToString(),
                dateRazdelenia);

            for (int col = 3; col <= 9; col++)
            {
                sheet.Cells[row, col].Value = GetPgGeneralColumnValue(rowData, col);
            }
        }

        sheet.Calculate();

        return await package.GetAsByteArrayAsync(cancellationToken);
    }

    public async Task<byte[]> GetPgFormGroupByMo(DateOnly from, DateOnly to, CancellationToken cancellationToken)
    {
        var regions = await dbContext.Regions.ToListAsync(cancellationToken);
        var moRegions = await dbContext.MoRegions.ToListAsync(cancellationToken);

        var reportData = await dbContext.Database
            .SqlQueryRaw<PgFormGroupByMoModel>("select * from sp_report_pg_by_mo({0}, {1})", from, to)
            .ToListAsync(cancellationToken);

        var regionsData = reportData.Where(c => c.RegionId != null);
        var withoutMoData = reportData.Where(c => c.RegionId == null);

        int moCounter = 1;
        int rowIndex = 7;

        var templatePath = Path.Combine(env.ContentRootPath, "Templates", "pg_by_mo.xlsx");
        ExcelPackage package = new ExcelPackage(templatePath);
        ExcelWorksheet sheet = package.Workbook.Worksheets.First();

        foreach (var region in regions)
        {
            var regionData = regionsData.Where(c => c.RegionId == region.Id);
            var regionMOs = moRegions.Where(c => c.RegionId == region.Id);
            var regionColor = GetColorFromPgByMoCellColor((PgByMoCellColor)(region.Id - 1));

            sheet.Cells[rowIndex, 1, rowIndex, 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            sheet.Cells[rowIndex, 1, rowIndex, 3].Style.Fill.BackgroundColor.SetColor(regionColor);

            sheet.Cells[rowIndex, 3].Value = region.Name;
            sheet.Cells[rowIndex, 3].Style.Font.Bold = true;
            sheet.Cells[rowIndex, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            rowIndex++;

            foreach (var mo in regionMOs)
            {
                var moData = regionData.Where(c => c.MoCode == mo.Code.ToString());

                sheet.Cells[rowIndex, 1].Value = moCounter;
                sheet.Cells[rowIndex, 1].Style.HorizontalAlignment =
                    OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[rowIndex, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheet.Cells[rowIndex, 1].Style.Fill.BackgroundColor.SetColor(regionColor);
                sheet.Cells[rowIndex, 1].Style.Font.Bold = true;

                sheet.Cells[rowIndex, 2].Value = mo.Code;

                PgFormByMoWriteRow(sheet, rowIndex, moData, mo.Name);

                rowIndex += 2;
                moCounter++;
            }

            PgFormByMoWriteRow(sheet, rowIndex, regionData, "Всего " + region.Name);

            sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Fill.BackgroundColor.SetColor(regionColor);

            rowIndex += 2;
        }

        sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Fill.BackgroundColor
            .SetColor(GetColorFromPgByMoCellColor((PgByMoCellColor)3));

        PgFormByMoWriteRow(sheet, rowIndex, regionsData, "Итого по районам");

        rowIndex += 2;

        PgFormByMoWriteRow(sheet, rowIndex, withoutMoData, "Без указания МО");

        rowIndex += 2;

        sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        sheet.Cells[rowIndex, 1, rowIndex, 21].Style.Fill.BackgroundColor
            .SetColor(GetColorFromPgByMoCellColor((PgByMoCellColor)3));

        PgFormByMoWriteRow(sheet, rowIndex, reportData, "ИТОГО");

        sheet.Cells[7, 1, rowIndex, 21].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        sheet.Cells[7, 1, rowIndex, 21].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        sheet.Cells[7, 1, rowIndex, 21].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        sheet.Cells[7, 1, rowIndex, 21].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        return await package.GetAsByteArrayAsync(cancellationToken);
    }

    public async Task<byte[]> GetOmsPerformanceCriteriaForm(int month, CancellationToken cancellationToken)
    {
        if (requestEnvironment.AuthUser == null)
        {
            throw new PermissionException("не авторизованный доступ");
        }

        var dateTime = new DateTime(DateTime.Now.Year, month, 1);

        ExcelPackage package = new ExcelPackage();
        ExcelWorksheet sheet = package.Workbook.Worksheets.Add(dateTime.ToString("MMMM"));

        sheet.Cells[1, 1].Value = "Расчет критериев эффективности ОМС за " + month + " " + dateTime.Year + " года";
        sheet.Cells[4, 2].Value = "Количество рассмотренных обращений граждан";
        sheet.Cells[4, 3].Value = "Из них поступило устно";
        sheet.Cells[4, 3].Style.WrapText = true;
        sheet.Cells[4, 4].Value = "Из них поступило письменно";
        sheet.Cells[4, 4].Style.WrapText = true;
        sheet.Cells[4, 5].Value = "В них недочетов";
        sheet.Cells[4, 5].Style.WrapText = true;
        sheet.Cells[4, 2].Style.WrapText = true;
        sheet.Cells[1, 1, 2, 2].Merge = true;
        sheet.Column(1).Width = 70;
        sheet.Column(2).Width = 20;
        sheet.Column(3).Width = 20;
        sheet.Column(4).Width = 20;
        sheet.Column(5).Width = 20;
        sheet.Cells[2, 1].Style.Font.Bold = true;
        sheet.Row(4).Height = 100;
        int counter = 0;
        int namer = 0;

        var data = await dbContext.Database
            .SqlQueryRaw<OmsPerformanceCriteriaFormModel>("select * from sp_get_by_oper({0}, {1}, {2})", month,
                dateTime.Year, requestEnvironment.AuthUser.OrgId)
            .ToListAsync(cancellationToken);

        for (int k = 1; k < 10; k++)
        {
            var foundRows = data.Where(c => c.Id == k).ToList();
            if (foundRows.Any())
            {
                sheet.Cells[5 + namer + counter, 1].Value = foundRows[0].Name;
                sheet.Cells[5 + namer + counter, 1].Style.Font.Bold = true;
                namer++;
                for (int i = 0; i < foundRows.Count; i++)
                {
                    sheet.Cells[5 + counter + namer, 1].Value = foundRows[i].FullName;
                    sheet.Cells[5 + counter + namer, 2].Value = Convert.ToInt32(foundRows[i].Coc);
                    sheet.Cells[5 + counter + namer, 3].Value = Convert.ToInt32(foundRows[i].Ust);
                    sheet.Cells[5 + counter + namer, 4].Value = Convert.ToInt32(foundRows[i].Pis);
                    sheet.Cells[5 + counter + namer, 5].Value = Convert.ToInt32(foundRows[i].Osh);
                    counter++;
                }
            }
        }

        return await package.GetAsByteArrayAsync(cancellationToken);
    }

    public async Task<byte[]> GetJustifiedComplaintsForm(DateOnly from, DateOnly to,
        CancellationToken cancellationToken)
    {
        var data = await dbContext.Database
            .SqlQueryRaw<JustifiedComplaintsFormModel>("select * from sp_report_minzdrav_jal_by_mo({0}, {1})", from, to)
            .ToListAsync(cancellationToken);

        var templatePath = Path.Combine(env.ContentRootPath, "Templates", "ObosJal.xlsx");
        ExcelPackage package = new ExcelPackage(templatePath);
        ExcelWorksheet sheet = package.Workbook.Worksheets.First();

        for (int i = 0; i < data.Count; i++)
        {
            sheet.Cells[6 + i, 2].Value = (i + 1).ToString();
            sheet.Cells[6 + i, 3].Value = data[i].MoName;
            sheet.Cells[6 + i, 4].Value = data[i].FormType;
            sheet.Cells[6 + i, 5].Value = data[i].FormName;
            sheet.Cells[6 + i, 6].SetDateValue(data[i].RegistrationDate);
            sheet.Cells[6 + i, 7].Value = data[i].Content;
            sheet.Cells[6 + i, 8].Value = data[i].WorkDone;
            sheet.Cells[6 + i, 9].Value = data[i].Id;
            sheet.Cells[6 + i, 10].Value = data[i].Theme;
            sheet.Cells[6 + i, 11].Value = data[i].TypeMpName;
        }

        sheet.Cells[3, 4].Value = from.ToString("dd.MM.yyyy") + "-" + to.ToString("dd.MM.yyyy");
        
        return await package.GetAsByteArrayAsync(cancellationToken);
    }

    public async Task<byte[]> GetAllComplaintsForm(DateOnly from, DateOnly to,
        CancellationToken cancellationToken)
    {
        var data = await dbContext.Database
            .SqlQueryRaw<AllComplaintsFormModel>("select * from sp_report_minzdrav_jal_by_mo2({0}, {1})", from, to)
            .ToListAsync(cancellationToken);
        
        var templatePath = Path.Combine(env.ContentRootPath, "Templates", "ObosJal.xlsx");
        ExcelPackage package = new ExcelPackage(templatePath);
        ExcelWorksheet sheet = package.Workbook.Worksheets.First();
        
        for (int i = 0; i < data.Count; i++)
        {
            sheet.Cells[6 + i, 2].Value = (i + 1).ToString();
            sheet.Cells[6 + i, 3].Value = data[i].MoName;
            sheet.Cells[6 + i, 4].Value = data[i].FormType;
            sheet.Cells[6 + i, 5].Value = data[i].FormName;
            sheet.Cells[6 + i, 6].SetDateValue(data[i].RegistrationDate);
            sheet.Cells[6 + i, 7].Value = data[i].Content;
            sheet.Cells[6 + i, 8].Value = data[i].WorkDone;
            sheet.Cells[6 + i, 9].Value = data[i].Id;
            sheet.Cells[6 + i, 10].Value = data[i].Theme;
            sheet.Cells[6 + i, 11].Value = data[i].TypeMpName;
            sheet.Cells[6 + i, 12].Value = data[i].IsRealJal;
        }

        sheet.Cells[3, 4].Value = from.ToString("dd.MM.yyyy") + "-" + to.ToString("dd.MM.yyyy");
        
        return await package.GetAsByteArrayAsync(cancellationToken);
    }

    private List<PgFormGeneralModel> GetPgGeneralRowData(List<PgFormGeneralModel> allData, int rowNum, string oldValue,
        string newValue, DateTime date_razdelenia)
    {
        if (rowNum == 9)
        {
            return allData;
        }

        if (rowNum == 10)
        {
            return allData.Where(c => c.TypeId == 2).ToList();
        }

        var newListRK = allData.Where(c => c.DateRegistration >= date_razdelenia);
        var oldListRK = allData.Where(c => c.DateRegistration < date_razdelenia);

        if (rowNum >= 11 && rowNum < 59)
        {
            var resOLD = new List<PgFormGeneralModel>();
            if (oldValue == "—")
            {
                resOLD = oldListRK.Where(c => c.Theme.StartsWith(newValue) && c.IsRealJal == 1).ToList();
            }
            else //if (oldValue != "-")
            {
                resOLD = oldListRK.Where(c => c.Theme.StartsWith(oldValue) && c.IsRealJal == 1).ToList();
            }

            var res = newListRK.Where(c => c.Theme.StartsWith(newValue) && c.IsRealJal == 1).ToList();

            foreach (var el in resOLD)
            {
                res.Add(el);
            }

            return res;
        }
        else //rows: 59-87
        {
            var resOLD = new List<PgFormGeneralModel>();
            if (oldValue == "—")
            {
                resOLD = oldListRK.Where(c => c.Theme.StartsWith(newValue)).ToList();
            }
            else
            {
                resOLD = oldListRK.Where(c => c.Theme.StartsWith(oldValue)).ToList();
            }

            var res = newListRK.Where(c => c.Theme.StartsWith(newValue)).ToList();


            foreach (var el in resOLD)
            {
                res.Add(el);
            }

            return res;
        }
    }

    private int GetPgGeneralColumnValue(List<PgFormGeneralModel> rowData, int col)
    {
        switch (col)
        {
            case 3:
                return rowData.Count;

            case 4: //ТФОМС Все
                return rowData.Count(d => d.OrganizationId >= 5);

            case 5: //ТФОМС Устных
                return rowData.Count(d => d.OrganizationId >= 5 && d.UstPis == 2);
                ;

            case 6: //ТФОМС Письменных
                return rowData.Count(d => d.OrganizationId >= 5 && d.UstPis == 1);
                ;

            case 7: //СМО Все
                return rowData.Count(d => d.OrganizationId < 5);

            case 8: //СМО Устных
                return rowData.Count(d => d.OrganizationId < 5 && d.UstPis == 2);

            case 9: //СМО Письменных
                return rowData.Count(d => d.OrganizationId < 5 && d.UstPis == 1);

            default:
                return 0;
        }
    }

    public Color GetColorFromPgByMoCellColor(PgByMoCellColor PgByMoCellColor)
    {
        switch (PgByMoCellColor)
        {
            case PgByMoCellColor.Violet:
                return System.Drawing.Color.Coral;
            case PgByMoCellColor.LightGreen:
                return System.Drawing.Color.LightGreen;
            case PgByMoCellColor.LightBrown:
                return System.Drawing.Color.BurlyWood;
            case PgByMoCellColor.Orange:
                return System.Drawing.Color.Orange;
            case PgByMoCellColor.VeryLightGreen:
                return System.Drawing.Color.PaleGreen;
            case PgByMoCellColor.Berezov:
                return System.Drawing.Color.DarkGreen;
            case PgByMoCellColor.Blue:
                return System.Drawing.Color.AliceBlue;
            case PgByMoCellColor.GreyGreen:
                return System.Drawing.Color.LawnGreen;
            case PgByMoCellColor.Poop:
                return System.Drawing.Color.Turquoise;
            case PgByMoCellColor.Red:
                return System.Drawing.Color.LightPink;
            case PgByMoCellColor.Yellow:
                return System.Drawing.Color.Yellow;
            case PgByMoCellColor.LightBlue:
                return System.Drawing.Color.Thistle;
            case PgByMoCellColor.DarkerBlue:
                return System.Drawing.Color.Teal;
            case PgByMoCellColor.none:
                return System.Drawing.Color.White;
            case PgByMoCellColor.SomeColor:
                return System.Drawing.Color.SlateGray;
            case PgByMoCellColor.AnotherColor:
                return System.Drawing.Color.Salmon;
            case PgByMoCellColor.NewColor:
                return System.Drawing.Color.SaddleBrown;
            case PgByMoCellColor.GoodColor:
                return System.Drawing.Color.SteelBlue;
            case PgByMoCellColor.PerfectColor:
                return System.Drawing.Color.OldLace;
            case PgByMoCellColor.OloloColor:
                return System.Drawing.Color.White;
            default:
                return System.Drawing.Color.White;
        }
    }

    private void PgFormByMoWriteRow(ExcelWorksheet sheet, int rowIndex, IEnumerable<PgFormGroupByMoModel> data,
        string name)
    {
        sheet.Cells[rowIndex, 3].Value = name;

        sheet.Cells[rowIndex, 4].Value = data.Count();
        sheet.Cells[rowIndex, 5].Value = data.Where(c => c.UstPis == 2).Count();
        for (int columnIndex = 6; columnIndex <= 21; columnIndex++)
        {
            sheet.Cells[rowIndex, columnIndex].Value =
                data.Where(c => c.ObrTheme.StartsWith(sheet.Cells[5, columnIndex].Value.ToString())).Count();
        }
    }
}