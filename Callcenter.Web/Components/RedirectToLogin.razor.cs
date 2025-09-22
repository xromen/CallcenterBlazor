using Microsoft.AspNetCore.Components;

namespace Callcenter.Web.Components;

public partial class RedirectToLogin : ComponentBase
{
    protected override void OnInitialized()
    {
        NavigationManager.NavigateTo($"Account/Login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}", true);
    }
}