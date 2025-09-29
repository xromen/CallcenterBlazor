namespace Callcenter.Shared.Requests;

public class SearchRequestDto
{
    public PaginatedRequestDto PaginatedRequest { get; set; } = new();

    public FilterRequestDto[]? Filters { get; set; }

    public OrderRequestDto[]? Orders { get; set; }
}