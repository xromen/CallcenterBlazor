using CallcenterBlazor.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace CallcenterBlazor.Components;

public partial class UserEditDialog : ComponentBase
{
    [Parameter] public UserModel User { get; set; } = new();
    
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }
    
    [Inject] private ISnackbar Snackbar { get; set; }
    
    private void Cancel() => MudDialog.Cancel();

    private void Delete()
    {
        Snackbar.Add("Пользователь удален", Severity.Success);
        MudDialog.Cancel();
    }

    private void Save()
    {
        Snackbar.Add("Пользователь сохранен", Severity.Success);
        MudDialog.Cancel();
    }
}