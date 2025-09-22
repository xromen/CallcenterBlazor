using Callcenter.Shared;
using Callcenter.Web.Pages;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Components;

public partial class SelectQuestionDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }

    [Inject] private ISnackbar Snackbar { get; set; }

    [Inject] private DeclarationsService Service { get; set; }

    [Inject] private ProblemDetailsHandler ProblemDetailsHandler { get; set; }

    private MudTable<QuestionDto> _mudTable;
    private List<QuestionGroupDto> _groups = new();
    private QuestionDto _selectedQuestion = new();
    private bool _isLoading = true;
    private bool _isAnswerEditing = false;
    private void Cancel() => MudDialog.Cancel();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        await base.OnInitializedAsync();

        var groupsResult = await Service.GetQuestionGroups();

        _isLoading = false;

        if (!groupsResult.Success)
        {
            ProblemDetailsHandler.Handle(groupsResult.Error!);
            return;
        }

        _groups = groupsResult.Data ?? new();
    }

    private string SelectedRowClassFunc(QuestionDto element, int rowNumber)
    {
        return _selectedQuestion.Id == element.Id ? "selected" : "";
    }

    private void RowClickEvent(TableRowClickEventArgs<QuestionDto> args)
    {
        _selectedQuestion = args.Item ?? new();
    }

    private Task AnswerEditModeChanged(bool value)
    {
        if (value)
        {
            _isAnswerEditing = true;
        }
        else
        {
            _isAnswerEditing = false;
            Snackbar.Add("Ответ на вопрос успешно сохранен", Severity.Success);
        }
        
        return Task.CompletedTask;
    }
}