using Callcenter.Shared;
using Callcenter.Web.Models;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;

namespace Callcenter.Web.Pages;

public partial class News : ComponentBase, IDisposable
{
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

    private async Task LoadNews(CancellationToken cancellationToken)
    {
        var result = await Service.GetNews(_news.Page, _news.PageSize, cancellationToken);

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

    public void Dispose()
    {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
    }
}