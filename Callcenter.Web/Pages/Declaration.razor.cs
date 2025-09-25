using Bogus;
using Callcenter.Shared;
using Callcenter.Web.Components;
using Callcenter.Web.Components.Dialogs;
using Callcenter.Web.Extensions;
using Callcenter.Web.Models;
using Callcenter.Web.Services;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Pages;

public partial class Declaration : ComponentBase
{
    [Parameter] public int? Id { get; set; }

    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    private DeclarationModel Model { get; set; } = new();

    private List<DeclarationActionDto>? Actions { get; set; }

    [Inject] private DeclarationsService Service { get; set; } = null!;

    [Inject] private ProblemDetailsHandler ProblemDetailsHandler { get; set; } = null!;

    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [Inject] private IDialogService Dialog { get; set; } = null!;

    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private DictionariesDto Dictionaries { get; set; } = new();

    private bool _isPersDataFilled =>
        !string.IsNullOrWhiteSpace(Model?.FathName) &&
        !string.IsNullOrWhiteSpace(Model?.FirstName) &&
        !string.IsNullOrWhiteSpace(Model?.SecName) &&
        Model?.BirthDate != null;

    private MudForm _form;
    private int? _typeContactForm;
    private bool _isLoading = true;
    private bool _isSaving = false;

    private List<IBrowserFile> _uploadedFiles = new();

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        var authState = await AuthenticationStateTask;
        var dictsResult = await Service.GetDictionaries();

        if (!dictsResult.Success)
        {
            ProblemDetailsHandler.Handle(dictsResult.Error!);
        }
        else
        {
            Dictionaries = dictsResult.Data!;
        }

        if (Id.HasValue)
        {
            var declarationResult = await Service.GetById(Id.Value);

            if (!declarationResult.Success)
            {
                ProblemDetailsHandler.Handle(declarationResult.Error!);
            }
            else
            {
                Model = declarationResult.Data!;
                
                if (Model.ContactFormId.HasValue)
                {
                    _typeContactForm = Dictionaries.VerbalContactForms.ContainsKey(Model.ContactFormId.Value) ? 1 :
                        Dictionaries.WritingContactForms.ContainsKey(Model.ContactFormId.Value) ? 2 : null;
                }

                // if (Model.InsuredMoId.HasValue)
                // {
                //     Model.InsuredMo = (await SearchInsuredMo(Model.InsuredMoId.ToString())).FirstOrDefault();
                // }
            }
        }
        else
        {
            if (authState.User.GetOrgId() > 4)
            {
                Model.DateRegistered = DateTime.Now;
            }
            else
            {
                Model.DateRegisteredSmo = DateTime.Now;
            }
        }

        await base.OnInitializedAsync();

        _isLoading = false;
    }

    private async Task TabPanelIndexChanged(int arg)
    {
        switch (arg)
        {
            case 2:
                if (Actions != null)
                {
                    break;
                }
                
                var result = await Service.GetHistory(Id!.Value);
                if (!result.Success)
                {
                    ProblemDetailsHandler.Handle(result.Error!);
                    return;
                }
                else
                {
                    Actions = result.Data!.OrderByDescending(d => d.Date).ToList();
                }

                break;
        }
    }

    private Task<IEnumerable<MoPhoneModel>> SearchMoPhone(string? query, CancellationToken cancellation)
    {
        var phoneNumbers = Dictionaries.MoPhoneNumbers.Select(c => new MoPhoneModel()
        {
            PhoneNumber = c.Key,
            Name = c.Value
        });

        return string.IsNullOrWhiteSpace(query)
            ? Task.FromResult(phoneNumbers.AsEnumerable())
            : Task.FromResult(phoneNumbers.Where(c =>
                $"{c.PhoneNumber} {c.Name}".Contains(query, StringComparison.InvariantCultureIgnoreCase)));
    }

    private Task<IEnumerable<int?>> SearchInsuredMo(string? query, CancellationToken cancellation = default)
    {
        var items = Dictionaries.MoOrganisations
            .Select(x => (int?)Convert.ToInt32(x.Key))
            .ToList();

        return string.IsNullOrWhiteSpace(query)
            ? Task.FromResult(items.AsEnumerable())
            : Task.FromResult(items.Where(id =>
                $"{id} {Dictionaries.MoOrganisations[id!.Value.ToString()]}"
                    .Contains(query, StringComparison.InvariantCultureIgnoreCase)));
    }

    private string? InsuredMoIdToString(int? moId)
    {
        if(moId == null) return null;
        return Dictionaries.MoOrganisations.TryGetValue(moId.Value.ToString(), out var name) ? name : moId.ToString();
    }

    private Task<IEnumerable<string>> SearchDeclarationTheme(string? query, CancellationToken arg2)
    {
        if (!Model.TypeId.HasValue)
        {
            Snackbar.Add("Вначале необходимо выбрать \"Вид обращения\"", Severity.Warning);
            return Task.FromResult<IEnumerable<string>>([]);
        }

        return string.IsNullOrWhiteSpace(query)
            ? Task.FromResult(Dictionaries.DeclarationThemes[Model.TypeId.Value].AsEnumerable())
            : Task.FromResult(Dictionaries.DeclarationThemes[Model.TypeId.Value]
                .Where(c => c.Contains(query, StringComparison.InvariantCultureIgnoreCase)));
    }

    private async Task QuestionsDialogOpen(MouseEventArgs arg)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Large, FullWidth = true, CloseButton = true};

        var dialog = await Dialog.ShowAsync<SelectQuestionDialog>("", options);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data! is QuestionDto question)
        {
            Model.WorkDone = question.Answer;
        }
    }

    private async Task SupervisorClose(MouseEventArgs arg)
    {
        DialogOptions options = new() { FullWidth = true, CloseButton = true};
        var parameters = new DialogParameters<SupervisorCloseDialog>
        {
            { x => x.DeclarationId, Id!.Value },
        };

        var dialog = await Dialog.ShowAsync<SupervisorCloseDialog>("Действия руководителя", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            Snackbar.Add("Карточка успешно закрыта", Severity.Success);
            Navigation.NavigateTo("/declarations");
        }
    }

    private async Task Save(MouseEventArgs arg)
    {
        _isSaving = true;
        try
        {
            //Валидация
            var errors = await ValidateModel();
            
            if (errors.Any(c => c.IsError))
            {
                await Dialog.ShowErrorsDialog(errors.Where(c => c.IsError).Select(c => c.Message));
                return;
            }

            if (errors.Any(c => c.IsWarning))
            {
                var confirmed = await Dialog.ShowWarningsDialog(errors.Where(c => c.IsWarning).Select(c => c.Message));

                if (!confirmed)
                    return;
            }
            
            if (Id.HasValue)
            {
                //Обновляем
                var response = await Service.UpdateDeclaration(Id!.Value, Model.Adapt<DeclarationDto>());

                if (!response.Success)
                {
                    ProblemDetailsHandler.Handle(response.Error!);
                    return;
                }
            }
            else
            {
                //Создаем новую
                var result = await Service.Add(Model);
                
                if (!result.Success)
                {
                    ProblemDetailsHandler.Handle(result.Error!);
                    return;
                }
            }

            foreach (var file in _uploadedFiles)
            {
                await Service.UploadFile(Model.Id, file);
            }

            Snackbar.Add("Карточка успешно сохранена", Severity.Success);

            await Task.Delay(500);

            Navigation.NavigateTo("/declarations");
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task<List<ValidationMessage>> ValidateModel()
    {
        var errors = new List<ValidationMessage>();
        if (Model.TypeId == null)
        {
            errors.Add(ValidationMessage.Error("Выберите вид обращения из списка"));
        }

        if (_typeContactForm == 2 && Model is { TypeId: 2, AnswerStatusId: 1, KemTypeId: null })
        {
            errors.Add(ValidationMessage.Error("Выберите вид проведенных КЭМ из списка"));
        }

        if (Model is { TypeId: 2, MpTypeId: null })
        {
            errors.Add(ValidationMessage.Error("Вы указали тип обращения 'Жалоба', но не выбрали вид оказанной МП"));
        }

        if ((Model.AnswerStatusId == 1 || Model.AnswerStatusId == 2) && string.IsNullOrWhiteSpace(Model.WorkDone))
        {
            errors.Add(ValidationMessage.Error("Вы указали состояние исполнения, но не написали принятые меры"));
        }

        if (Model is { AnswerStatusId: 2, ResultId: null })
        {
            errors.Add(ValidationMessage.Error("Вы дали окончательный ответ, но не выбрали результат обращения"));
        }

        if (Model is { AnswerStatusId: 2, TypeId: 2, SvedJalId: null })
        {
            errors.Add(ValidationMessage.Error("Вы дали окончательный ответ на жалобу, но не выбрали сведения о ней"));
        }

        if (Model.SourceId == null)
        {
            errors.Add(ValidationMessage.Error("Выберите источник поступления из списка"));
        }

        if (Model.ContactFormId == null)
        {
            errors.Add(ValidationMessage.Error("Выберите форму обращения из списка"));
        }

        if (string.IsNullOrWhiteSpace(Model.Content))
        {
            errors.Add(ValidationMessage.Error("Вы не написали содержание обращения"));
        }

        if (string.IsNullOrWhiteSpace(Model.Theme))
        {
            errors.Add(ValidationMessage.Error("Вы не указали тему обращения"));
        }

        if (_typeContactForm == 2 &&
            (string.IsNullOrWhiteSpace(Model.FirstName) || string.IsNullOrWhiteSpace(Model.SecName)))
        {
            errors.Add(ValidationMessage.Warning("Вы указали письменное обращение без ФИО обратившегося"));
        }

        if (_typeContactForm == 2 && Model.InsuredSmoId == null)
        {
            errors.Add(ValidationMessage.Warning("Вы указали письменное обращение без СМО обратившегося"));
        }

        if (Id == null && !string.IsNullOrWhiteSpace(Model.FirstName) && !string.IsNullOrWhiteSpace(Model.SecName) &&
            Model.BirthDate.HasValue)
        {
            var result = await Service.FindDeclarationsByFio(Model.FirstName, Model.SecName, Model.BirthDate.Value);

            if (!result.Success)
            {
                ProblemDetailsHandler.Handle(result.Error!);
            }
            
            if(result.Data != null && result.Data.Any())
            {
                errors.Add(ValidationMessage.Warning($"Найдены карточки по указанным данным обратившегося (Номера: {string.Join(", ", result.Data)}).Возможно вы пытаетесь создать повторную карточку!"));
            }
        }
        
        if (Id == null && !string.IsNullOrWhiteSpace(Model.EjogNumber))
        {
            var result = await Service.FindDeclarationsByEjogNumber(Model.EjogNumber);

            if (!result.Success)
            {
                ProblemDetailsHandler.Handle(result.Error!);
                return errors;
            }

            if (result.Data != null)
            {
                errors.Add(ValidationMessage.Error("Карточка с данным номером ЭЖОГ добавлена ранее"));
            }
        }

        return errors;
    }
}