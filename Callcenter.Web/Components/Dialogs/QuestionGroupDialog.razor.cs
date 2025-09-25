using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Callcenter.Web.Components.Dialogs;

public partial class QuestionGroupDialog : ComponentBase
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    
    [Parameter]
    public int? QuestionGroupId { get; set; }
    
    [Parameter]
    public string? QuestionGroupName { get; set; }
    
    [Parameter]
    public Func<QuestionGroupEventArgs, Task<bool>> OnSubmit { get; set; }

    [Inject] public DeclarationsService Service { get; set; } = null!;
    
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    
    [Inject] public ProblemDetailsHandler ProblemDetailsHandler { get; set; } = null!;

    private void Cancel() => MudDialog.Cancel();

    private async Task Submit()
    {
        var args = new QuestionGroupEventArgs()
        {
            Id = QuestionGroupId,
            Name = QuestionGroupName
        };

        var success = await OnSubmit(args);
        
        if(success)
            MudDialog.Close(DialogResult.Ok(QuestionGroupName));
    }
}

public class QuestionGroupEventArgs
{
    public int? Id { get; set; }
    public string Name { get; set; }
}