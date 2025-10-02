using Callcenter.Web.Extensions;
using Callcenter.Web.Services.Authentication;
using Microsoft.AspNetCore.Components;

namespace Callcenter.Web.Pages.Account;

public partial class Logout : ComponentBase, IDisposable
{
    [SupplyParameterFromQuery]
    private string? ReturnUrl { get; set; }

    [Inject]
    private AuthenticationService AuthenticationService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;
    
    private CancellationTokenSource _tokenSource = new();

    protected override async Task OnInitializedAsync()
    {
        await AuthenticationService.LogoutAsync(_tokenSource.Token);
        NavigationManager.ReturnTo(ReturnUrl, true);
    }

    public void Dispose()
    {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
    }
}