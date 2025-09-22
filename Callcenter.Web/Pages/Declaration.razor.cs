using Bogus;
using Callcenter.Shared;
using Callcenter.Web.Components;
using Callcenter.Web.Extensions;
using Callcenter.Web.Models;
using Callcenter.Web.Services;
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

    private DictionariesDto Dictionaries { get; set; } = new();

    private MudForm _form;
    private int? _typeContactForm;
    private bool _isLoading = true;

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
                var result = await Service.GetHistory(Id!.Value);
                if (!result.Success)
                {
                    ProblemDetailsHandler.Handle(result.Error!);
                    return;
                }
                else
                {
                    Actions = result.Data!;
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

    private Task<IEnumerable<MoOrganisationModel>> SearchInsuredMo(string? query, CancellationToken arg2)
    {
        var moOrganisations = Dictionaries.MoOrganisations.Select(c => new MoOrganisationModel()
        {
            MoCode = c.Key,
            MoName = c.Value
        }).ToList();

        moOrganisations.Insert(0, new() { MoCode = null, MoName = "Без указания МО" });

        return string.IsNullOrWhiteSpace(query)
            ? Task.FromResult(moOrganisations.AsEnumerable())
            : Task.FromResult(moOrganisations.Where(c =>
                $"{c.MoCode} {c.MoName}".Contains(query, StringComparison.InvariantCultureIgnoreCase)));
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
        DialogOptions options = new() { MaxWidth = MaxWidth.Large, FullWidth = true};
        
        var dialog = await Dialog.ShowAsync<SelectQuestionDialog>("", options);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data! is QuestionDto question)
        {
            Model.WorkDone = question.Answer;
        }
    }
}