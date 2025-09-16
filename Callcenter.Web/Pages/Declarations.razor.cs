using Callcenter.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Pages;

public partial class Declarations : ComponentBase
{
    [Inject] private IDialogService Dialog { get; set; } = null!;
    
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    
    private static Random _rnd = new Random();
    
    private List<DeclarationModel> _declarations =
        [
            new()
            {
                Number = "ХКФОМС-1",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-2",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme =
                    "3.1.7.2 при прохождении диспансеризации (за исключением диспансеризации несовершеннолетних), всего, из них:aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-3",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-4",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-5",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-6",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-7",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-8",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-9",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-10",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-11",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-12",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-13",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-14",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
            new()
            {
                Number = "ХКФОМС-15",
                Date = new DateOnly(2025, _rnd.Next(1, 12), _rnd.Next(1, 28)),
                Theme = "3.1.3 на нарушение прав на выбор медицинской организации",
                Status = "отправлен ответ"
            },
        ];
    
    private PaginateModel<DeclarationModel> _paginateModel = new();
    
    private bool _isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        for (int i = 0; i < _declarations.Count; i++)
        {
            _declarations[i].Id = i;
        }
        
        await LoadItems();
    }

    private async Task LoadItems()
    {
        _isLoading = true;
        
        await Task.Delay(1000);
        
        _paginateModel.Items = _declarations.OrderByDescending(c => c.Date).Skip(_paginateModel.PageSize * (_paginateModel.Page - 1)).Take(_paginateModel.PageSize).ToList();
        _paginateModel.ItemsCount = _declarations.Count();
        
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

    private async Task SendCard(DeclarationModel declaration)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Medium, FullWidth = true };
        var parameters = new DialogParameters<SendCardDialog> { { x => x.Declaration, declaration} };
        
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

public class DeclarationModel
{
    public long Id { get; set; }
    public string Number { get; set; }
    public DateOnly Date { get; set; }
    public string Theme { get; set; }
    public string Status { get; set; }
}

public class PaginateModel<T>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int ItemsCount { get; set; }
    public List<T> Items { get; set; } = new();
}