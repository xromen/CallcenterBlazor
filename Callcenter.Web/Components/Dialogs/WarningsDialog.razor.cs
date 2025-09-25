using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Components.Dialogs;

public partial class WarningsDialog : ComponentBase
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }
    
    [Parameter]
    public IEnumerable<string> Warnings { get; set; } = new List<string>();
    
    private void Cancel() => MudDialog.Cancel();
    
    private void Save() => MudDialog.Close(DialogResult.Ok(true));
}