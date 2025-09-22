namespace Callcenter.Shared.Requests;

public class PaginatedRequestDto
{
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}