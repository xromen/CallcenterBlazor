using Callcenter.Web.Components;
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

    private List<DeclarationListDto> _declarations;
    
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
        
        var declarationsResult = await Service.GetList(_paginateModel.PageSize, _paginateModel.Page);

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
        var parameters = new DialogParameters<SendCardDialog> { { x => x.Declaration, declarationList} };
        
        await Dialog.ShowAsync<SendCardDialog>("Custom Options Dialog", parameters, options);
    }

    private async Task SearchCard()
    {
        if (_searchNumber == null)
        {
            Snackbar.Add("Вначале введите номер карточки", Severity.Error);
            return;
        }
        _isLoading = true;
        
        _paginateModel.Items = _declarations.Where(c => c.Number.Split('-').Last() == _searchNumber.ToString()).ToList();
        _paginateModel.ItemsCount = 1;
        _paginateModel.Page = 1;
        
        _isLoading = false;
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
}