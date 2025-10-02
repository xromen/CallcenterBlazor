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

public partial class Declarations : ComponentBase, IDisposable
{
    [Inject] private IDialogService Dialog { get; set; } = null!;
    
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    
    [Inject] private DeclarationsService Service { get; set; } = null!;
    
    [Inject] private ProblemDetailsHandler ProblemDetailsHandler { get; set; } = null!;
    
    private PaginateModel<DeclarationListDto> _paginateModel = new();
    
    private bool _isLoading = false;

    private long _answerSendCount = 0;
    private long _needsReworkCount = 0;
    private long _smoRedirectCount = 0;
    
    private CancellationTokenSource _tokenSource = new();

    private string GetPanelStyleByStatusId(int statusId) =>
        statusId switch
        {
            3 => "background-color:#d6eba0;",//перенаправлен в смо
            10 => "background-color:orange;",//требует доработок
            _ => string.Empty
        };
    
    private string GetStatusTextStyleByStatusId(int statusId) =>
        statusId switch
        {
            2 => "color:#ff5722;",//в обработке
            4 => "color:#673ab7;",//отправлен промежуточный ответ
            5 => "color:#673ab7;",//отправлен ответ
            7 => "color:#212121;",//закрыт администратором
            _ => string.Empty
        };

    protected override async Task OnInitializedAsync()
    {
        await LoadItems(_tokenSource.Token);
    }

    private async Task LoadItems(CancellationToken cancellationToken)
    {
        _isLoading = true;
        
        var declarationsResult = await Service.GetList(_paginateModel.PageSize, _paginateModel.Page, _searchNumber.ToString(), cancellationToken);

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
        await LoadItems(_tokenSource.Token);
    }

    private async Task PageSizeChanged(IEnumerable<int> obj)
    {
        _paginateModel.PageSize = obj.First();
        _paginateModel.Page = 1;
        await LoadItems(_tokenSource.Token);
    }

    private async Task SendCard(DeclarationListDto declarationList)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Medium, FullWidth = true };
        var parameters = new DialogParameters<SendCardDialog> { { x => x.DeclarationId, declarationList.Id} };
        
        var dialog = await Dialog.ShowAsync<SendCardDialog>("Custom Options Dialog", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await LoadItems(_tokenSource.Token);
        }
    }

    private async Task SearchCard()
    {
        if (_searchNumber == null)
        {
            Snackbar.Add("Вначале введите номер карточки", Severity.Error);
            return;
        }
        
        await LoadItems(_tokenSource.Token);
    }

    private int? _searchNumber = null;
    private async Task SearchNumberChanged(int? arg)
    {
        _searchNumber = arg;
        
        if (arg == null)
        {
            await LoadItems(_tokenSource.Token);
        }
    }

    private async Task SendToDelete(int id)
    {
        var confirmed = await Dialog.ShowConfirmDialog("Вы уверены что хотите удалить карточку?", "Удалить", Color.Error);
        
        if(!confirmed)
            return;
        
        var result = await Service.Remove(id, _tokenSource.Token);

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            return;
        }
        
        Snackbar.Add("Карточка успешно отправлена на удаление", Severity.Success);
        await LoadItems(_tokenSource.Token);
    }

    public void Dispose()
    {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
        Snackbar.Dispose();
    }
}