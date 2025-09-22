using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Callcenter.Web.Layout;

public partial class NavMenu : ComponentBase
{
    [Parameter]
    public string? Style { get; set; }
    
    [Parameter]
    public string? Class { get; set; }
    
    [CascadingParameter]
    public Task<AuthenticationState> AuthenticationStateTask { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var authState = await AuthenticationStateTask;
            
            StateHasChanged();
        }
        
        await base.OnAfterRenderAsync(firstRender);
    }
}