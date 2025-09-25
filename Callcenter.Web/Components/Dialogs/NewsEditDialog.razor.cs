using Callcenter.Web.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Components.Dialogs;

public partial class NewsEditDialog : ComponentBase
{
    [Parameter] public NewsDto NewsObj { get; set; } = new();
    
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }
    
    [Inject] private ISnackbar Snackbar { get; set; }
    
    private void Cancel() => MudDialog.Cancel();

    private void Delete()
    {
        Snackbar.Add("Новость удалена", Severity.Success);
        MudDialog.Cancel();
    }

    private void Save()
    {
        Snackbar.Add("Новость сохранена", Severity.Success);
        MudDialog.Cancel();
    } 

    private void OrgCheckedChanged(bool value, string org)
    {
        if (value)
        {
            NewsObj.Orgs.Add(org);
        }
        else
        {
            NewsObj.Orgs.Remove(org);
        }
    }
}