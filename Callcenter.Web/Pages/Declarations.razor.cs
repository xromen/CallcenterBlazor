using Callcenter.Web.Components;
using Callcenter.Web.Components.Dialogs;
using Callcenter.Web.Extensions;
using Callcenter.Web.Models;
using Callcenter.Web.Services;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Pages;

public partial class Declarations : ComponentBase
{
    [Inject] private IDialogService Dialog { get; set; } = null!;
    
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    
    [Inject] private DeclarationsService Service { get; set; } = null!;
    
    [Inject] private ProblemDetailsHandler ProblemDetailsHandler { get; set; } = null!;
    
    private static Random _rnd = new Random();
    
    private PaginateModel<DeclarationListDto> _paginateModel = new();
    
    private bool _isLoading = false;

    private long _answerSendCount = 0;
    private long _needsReworkCount = 0;
    private long _smoRedirectCount = 0;

    protected override async Task OnInitializedAsync()
    {
        await LoadItems();
    }

    private async Task LoadItems()
    {
        _isLoading = true;
        
        var declarationsResult = await Service.GetList(_paginateModel.PageSize, _paginateModel.Page, _searchNumber.ToString());

        if (!declarationsResult.Success)
        {
            ProblemDetailsHandler.Handle(declarationsResult.Error!);
            return;
        }
        
        var data = declarationsResult.Data!;
        
        _paginateModel.Items =  data.Declarations.ToList().Adapt<List<DeclarationListDto>>();
        _paginateModel.ItemsCount = data.TotalDeclarationsItems;

        _answerSendCount = data.Statistics.SendAnswerCount;
        _smoRedirectCount = data.Statistics.SmoRedirectCount;
        _needsReworkCount = data.Statistics.NeedReworkCount;
        
        _isLoading = false;
    }

    private async Task SelectedPageChanged(int obj)
    {
        _paginateModel.Page = obj;
        await LoadItems();
    }

    private async Task PageSizeChanged(IEnumerable<int> obj)
    {
        _paginateModel.PageSize = obj.First();
        _paginateModel.Page = 1;
        await LoadItems();
    }

    private async Task SendCard(DeclarationListDto declarationList)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Medium, FullWidth = true };
        var parameters = new DialogParameters<SendCardDialog> { { x => x.DeclarationId, declarationList.Id} };
        
        var dialog = await Dialog.ShowAsync<SendCardDialog>("Custom Options Dialog", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await LoadItems();
        }
    }

    private async Task SearchCard()
    {
        if (_searchNumber == null)
        {
            Snackbar.Add("Вначале введите номер карточки", Severity.Error);
            return;
        }
        
        await LoadItems();
    }

    private int? _searchNumber = null;
    private async Task SearchNumberChanged(int? arg)
    {
        _searchNumber = arg;
        
        if (arg == null)
        {
            await LoadItems();
        }
    }

    private async Task SendToDelete(int id)
    {
        var confirmed = await Dialog.ShowConfirmDialog("Вы уверены что хотите удалить карточку?", "Удалить", Color.Error);
        
        if(!confirmed)
            return;
        
        var result = await Service.Remove(id);

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            return;
        }
        
        Snackbar.Add("Карточка успешно отправлена на удаление", Severity.Success);
        await LoadItems();
    }
}