using Callcenter.Shared;
using Callcenter.Web.Extensions;
using Callcenter.Web.Pages;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Components.Dialogs;

public partial class SelectQuestionDialog : ComponentBase, IDisposable
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Inject] private DeclarationsService Service { get; set; } = null!;

    [Inject] private ProblemDetailsHandler ProblemDetailsHandler { get; set; } = null!;
    
    [Inject] private IDialogService DialogService { get; set; } = null!;

    private MudTable<QuestionDto> _mudTable;
    private List<QuestionGroupDto> _groups = new();
    private QuestionDto _selectedQuestion = new();
    private bool _isLoading = true;
    private bool _isAnswerEditing = false;
    private void Cancel() => MudDialog.Cancel();
    
    private CancellationTokenSource _tokenSource = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await LoadGroups();
    }

    private async Task LoadGroups()
    {
        _isLoading = true;
        
        var groupsResult = await Service.GetQuestionGroups(_tokenSource.Token);

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

    private async Task AnswerEditModeChanged(bool value)
    {
        if (value)
        {
            _isAnswerEditing = true;
        }
        else
        {
            _isAnswerEditing = false;
            
            var response = await Service.ChangeQuestionAnswer(_selectedQuestion.Id, _selectedQuestion.Answer, _tokenSource.Token);

            if (response.Success)
            {
                //_groups.Single(c => c.Id == _selectedQuestion.GroupId).Questions
                Snackbar.Add("Ответ на вопрос успешно сохранен", Severity.Success);
            }
            else
            {
                ProblemDetailsHandler.Handle(response.Error!);
            }
        }
    }

    private async Task DeleteQuestion(MouseEventArgs arg)
    {
        var confirmed =
            await DialogService.ShowConfirmDialog("Вы действительно хотите удалить этот вопрос?", "Удалить",
                Color.Error);
        
        if (!confirmed)
            return;
        
        var response = await Service.DeleteQuestion(_selectedQuestion.Id, _tokenSource.Token);

        if (response.Success)
        {
            _groups.Single(c => c.Id == _selectedQuestion.GroupId).Questions.Remove(_selectedQuestion);
            _selectedQuestion = new();
            Snackbar.Add("Вопрос успешно удален", Severity.Success);
        }
        else
        {
            ProblemDetailsHandler.Handle(response.Error!);
        }
    }

    private async Task OpenRenameQuestionGroupDialog(QuestionGroupDto group)
    {
        var parameters = new DialogParameters<QuestionGroupDialog>
        {
            { x => x.QuestionGroupId, group.Id },
            { x => x.QuestionGroupName, group.Name },
            { x => x.OnSubmit, RenameQuestionGroup },
        };
        DialogOptions options = new() { CloseButton = true, FullWidth = true};

        var dialog = await DialogService.ShowAsync<QuestionGroupDialog>("Редактирование группы", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data is string newGroupName)
        {
            _groups.Single(c => c.Id == group.Id).Name = newGroupName;
            StateHasChanged();
        }
    }

    private async Task OpenCreateQuestionGroupDialog()
    {
        var parameters = new DialogParameters<QuestionGroupDialog>
        {
            { x => x.OnSubmit, CreateQuestionGroup },
        };

        var dialog = await DialogService.ShowAsync<QuestionGroupDialog>("Создание группы", parameters);
    }

    private async Task OpenCreateQuestionDialog()
    {
        var parameters = new DialogParameters<QuestionDialog>
        {
            { x => x.OnSubmit, CreateQuestion },
            { x => x.AvailableGroups, _groups },
        };

        var dialog = await DialogService.ShowAsync<QuestionDialog>("Создание вопроса", parameters);
    }

    private async Task<bool> CreateQuestion(QuestionDto question, CancellationToken cancellationToken)
    {
        var response = await Service.CreateQuestion(question.GroupId, question.Name, question.Answer, cancellationToken);

        if (!response.Success)
        {
            ProblemDetailsHandler.Handle(response.Error!);
            return false;
        }
        
        _groups.Single(c => c.Id == question.GroupId).Questions.Add(response.Data!);
        Snackbar.Add("Вопрос успешно создался", Severity.Success);
        
        return true;
    }

    private async Task<bool> CreateQuestionGroup(QuestionGroupEventArgs args, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(args.Name))
        {
            Snackbar.Add("Имя группы не может быть пустым", Severity.Warning);
            return false;
        }

        var response = await Service.CreateQuestionGroup(args.Name, cancellationToken);

        if (!response.Success)
        {
            ProblemDetailsHandler.Handle(response.Error!);
            return false;
        }
        
        _groups.Add(response.Data!);
        StateHasChanged();
        Snackbar.Add("Группа успешно создалась", Severity.Success);
        
        return true;
    }
    
    private async Task<bool> RenameQuestionGroup(QuestionGroupEventArgs args, CancellationToken cancellationToken)
    {
        if (!args.Id.HasValue)
        {
            Snackbar.Add("Произошла ошибка, попробуйте позже", Severity.Error);
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(args.Name))
        {
            Snackbar.Add("Имя группы не может быть пустым", Severity.Warning);
            return false;
        }
        
        var response = await Service.RenameQuestionGroup(args.Id.Value, args.Name, cancellationToken);

        if (!response.Success)
        {
            ProblemDetailsHandler.Handle(response.Error!);
            return false;
        }
        
        Snackbar.Add("Группа успешно переименована", Severity.Success);
        return true;
    }

    public void Dispose()
    {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
        _mudTable.Dispose();
        Snackbar.Dispose();
    }
}