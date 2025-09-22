namespace Callcenter.Web.Models;

public class PaginateModel<T>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public long ItemsCount { get; set; }
    public List<T> Items { get; set; } = new();
}