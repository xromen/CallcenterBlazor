using System.Net.Http.Headers;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Callcenter.Web.Components.Dialogs;

public partial class QuestionGroupDialog : ComponentBase, IDisposable
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    
    [Parameter]
    public int? QuestionGroupId { get; set; }
    
    [Parameter]
    public string? QuestionGroupName { get; set; }
    
    [Parameter]
    public Func<QuestionGroupEventArgs, CancellationToken, Task<bool>> OnSubmit { get; set; }

    [Inject] public DeclarationsService Service { get; set; } = null!;
    
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    
    [Inject] public ProblemDetailsHandler ProblemDetailsHandler { get; set; } = null!;

    private CancellationTokenSource _tokenSource = new();
    
    private void Cancel() => MudDialog.Cancel();

    private async Task Submit()
    {
        var args = new QuestionGroupEventArgs()
        {
            Id = QuestionGroupId,
            Name = QuestionGroupName
        };

        var success = await OnSubmit(args, _tokenSource.Token);
        
        if(success)
            MudDialog.Close(DialogResult.Ok(QuestionGroupName));
    }

    public void Dispose()
    {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
        Snackbar.Dispose();
    }
}

public class QuestionGroupEventArgs
{
    public int? Id { get; set; }
    public string Name { get; set; }
}