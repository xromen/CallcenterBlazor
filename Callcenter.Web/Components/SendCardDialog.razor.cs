using Callcenter.Web.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Components;

public partial class SendCardDialog : ComponentBase
{
    [Parameter] public DeclarationModel Declaration { get; set; }
    
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }
    
    [Inject] private ISnackbar Snackbar { get; set; }

    private string _selectedOrg;
    
    private void Cancel() => MudDialog.Cancel();

    private void Send()
    {
        Snackbar.Add("Карточка отправлена", Severity.Success);
        MudDialog.Cancel();
    }

    private void ExpandedOrgChanged(bool value, string org)
    {
        if (value)
        {
            _selectedOrg = org;
        }
    }
}