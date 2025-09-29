using Callcenter.Shared;
using Callcenter.Web.Models;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;

namespace Callcenter.Web.Pages;

public partial class News : ComponentBase
{
    [Inject] public NewsService Service { get; set; } = null!;
    
    [Inject] public ProblemDetailsHandler ProblemDetailsHandler { get; set; } = null!;

    private PaginateModel<NewsDto> _news = new()
    {
        PageSize = 5
    };

    protected override async Task OnInitializedAsync()
    {
        await LoadNews();
    }

    private async Task LoadNews()
    {
        var result = await Service.GetNews(_news.Page, _news.PageSize);

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
        
        return LoadNews();
    }
}