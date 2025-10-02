using Callcenter.Shared;
using Callcenter.Web.Models;
using Callcenter.Web.Pages;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Callcenter.Web.Components;

public partial class NewsTable : ComponentBase, IDisposable
{
    [Parameter] public EventCallback<NewsClickEventArgs> OnRowClick { get; set; }

    [Inject] public NewsService Service { get; set; } = null!;

    [Inject] public ProblemDetailsHandler ProblemDetailsHandler { get; set; } = null!;

    private CancellationTokenSource _tokenSource = new();

    private PaginateModel<NewsDto> _news = new()
    {
        PageSize = 5
    };

    protected override async Task OnInitializedAsync()
    {
        await LoadNews(_tokenSource.Token);
    }

    public async Task LoadNews(CancellationToken ct)
    {
        var result = await Service.GetNews(_news.Page, _news.PageSize, ct);

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            return;
        }

        _news.ItemsCount = result.Data!.TotalItems;
        _news.Items = result.Data!.Items;
    }

    private Task SelectedPageChanged(int page)
    {
        _news.Page = page;

        return LoadNews(_tokenSource.Token);
    }

    private async Task RowClickEvent(MouseEventArgs arg, NewsDto dto)
    {
        await OnRowClick.InvokeAsync(new()
        {
            MouseEventArgs = arg,
            Item = dto
        });
    }

    public void Dispose()
    {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
    }
}

public class NewsClickEventArgs
{
    public MouseEventArgs MouseEventArgs { get; set; }
    public NewsDto Item { get; set; }
}