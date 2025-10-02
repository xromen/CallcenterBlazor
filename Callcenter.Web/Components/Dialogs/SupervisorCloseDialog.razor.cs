using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Components.Dialogs;

public partial class SupervisorCloseDialog : ComponentBase, IDisposable
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    
    [Parameter]
    public int DeclarationId { get; set; }
    
    [Inject] DeclarationsService Service { get; set; }
    
    [Inject] ProblemDetailsHandler ProblemDetailsHandler { get; set; }
    
    private string? _notes;
    
    private CancellationTokenSource _tokenSource = new();

    private Task SupervisorClose(MouseEventArgs arg) => SendRequest(false);

    private Task SendNote(MouseEventArgs arg) => SendRequest(true);

    private async Task SendRequest(bool isBad)
    {
        var result = await Service.SupervisorClose(DeclarationId, _notes, isBad, _tokenSource.Token);

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
        }
        else
        {
            MudDialog.Close();
        }
    }

    public void Dispose()
    {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
    }
}