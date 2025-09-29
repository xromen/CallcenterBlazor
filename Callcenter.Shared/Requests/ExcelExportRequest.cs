namespace Callcenter.Shared.Requests;

public class ExcelExportRequest
{
    public FilterRequestDto[]? Filters { get; set; }

    public OrderRequestDto[]? Orders { get; set; }
}