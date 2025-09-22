using Callcenter.Web.Extensions;
using Callcenter.Web.Services.Authentication;
using Microsoft.AspNetCore.Components;

namespace Callcenter.Web.Pages.Account;

public partial class Logout : ComponentBase
{
    [SupplyParameterFromQuery]
    private string? ReturnUrl { get; set; }

    [Inject]
    private AuthenticationService AuthenticationService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await AuthenticationService.LogoutAsync();
        NavigationManager.ReturnTo(ReturnUrl, true);
    }
}