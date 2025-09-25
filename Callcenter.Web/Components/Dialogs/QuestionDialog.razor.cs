using Callcenter.Shared;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Callcenter.Web.Components.Dialogs;

public partial class QuestionDialog : ComponentBase
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    
    [Parameter]
    public List<QuestionGroupDto> AvailableGroups { get; set; }
    
    [Parameter]
    public Func<QuestionDto, Task<bool>> OnSubmit { get; set; }

    [Inject] public DeclarationsService Service { get; set; } = null!;
    
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    
    [Inject] public ProblemDetailsHandler ProblemDetailsHandler { get; set; } = null!;

    private int? _groupId;
    private string? _questionName;
    private string? _questionAnswer;
    
    private void Cancel() => MudDialog.Cancel();

    private async Task Submit()
    {
        if (_groupId == null)
        {
            Snackbar.Add("Вначале выберите группу", Severity.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(_questionName) || string.IsNullOrWhiteSpace(_questionAnswer))
        {
            Snackbar.Add("Имя вопроса и ответ не должны быть пустыми", Severity.Warning);
            return;
        }
        
        var args = new QuestionDto()
        {
            GroupId = _groupId.Value,
            Name = _questionName,
            Answer = _questionAnswer
        };

        var success = await OnSubmit(args);
        
        if(success)
            MudDialog.Close(DialogResult.Ok(true));
    }
}