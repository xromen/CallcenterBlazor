using System.ComponentModel.DataAnnotations;
using Callcenter.Web.Extensions;
using Callcenter.Web.Models;
using Callcenter.Web.Services.Authentication;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Callcenter.Web.Pages.Account;

public partial class Login : ComponentBase, IDisposable
{
    [SupplyParameterFromForm]
    private InputModel Input { get; set; } = null!;

    [SupplyParameterFromQuery]
    private string? ReturnUrl { get; set; }

    [Inject]
    private AuthenticationService AuthenticationService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;
    
    private bool _isLoading = false;
    
    private CancellationTokenSource _tokenSource = new();

    public async Task LoginUserAsync(EditContext context)
    {
        _isLoading = true;
        try
        {
            var user = new UserDto(Input.Login, Input.Password);

            await AuthenticationService.LoginAsync(user, _tokenSource.Token)
                .TapError(message => Snackbar.Add($"Ошибка во время входа {message}", Severity.Error))
                .Map(() => ReturnUrl ?? "")
                .Tap(x => NavigationManager.ReturnTo(x));
        }
        finally
        {
            _isLoading = false;
        }
    }

    protected override void OnParametersSet()
    {
        Input = new();
    }

    private sealed class InputModel
    {
        [Required(ErrorMessage = "Логин обязателен.")]
        [Display(Name = "Логин или Email")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;
    }

    public void Dispose()
    {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
        Snackbar.Dispose();
    }
}