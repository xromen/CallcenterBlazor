using Callcenter.Web.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Callcenter.Web.Components;

public partial class NewsTable : ComponentBase
{
    [Parameter]
    public List<NewsDto> NewsDtos { get; set; }
    
    [Parameter]
    public EventCallback<NewsClickEventArgs> OnRowClick { get; set; }

    private async Task RowClickEvent(MouseEventArgs arg, NewsDto dto)
    {
        await OnRowClick.InvokeAsync(new()
        {
            MouseEventArgs = arg,
            Item = dto
        });
        
    }
}

public class NewsClickEventArgs
{
    public MouseEventArgs MouseEventArgs { get; set; }
    public NewsDto Item { get; set; }
}