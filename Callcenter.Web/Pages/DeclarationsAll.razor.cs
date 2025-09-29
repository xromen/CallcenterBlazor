using Callcenter.Shared;
using Callcenter.Shared.Requests;
using Callcenter.Shared.Responses;
using Callcenter.Web.Components;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Callcenter.Web.Pages;

public partial class DeclarationsAll : ComponentBase
{
    
    [Inject] DeclarationsService Service { get; set; }
    
    [Inject] ProblemDetailsHandler ProblemDetailsHandler { get; set; }
    
    [Inject] NavigationManager NavigationManager { get; set; }
    
    [Inject] IJSRuntime Js { get; set; }
    
    private int selectedRowNumber = -1;
    private FlexDataGrid<DeclarationDto> _mudDataGrid;
    private List<IFilterDefinition<DeclarationDto>> _filters = new();

    protected override void OnInitialized()
    {
        
    }
    private void RowClickEvent(DataGridRowClickEventArgs<DeclarationDto> args)
    {
        if (args.MouseEventArgs.Detail > 1)
        {
            NavigationManager.NavigateTo($"/Declaration/{args.Item.Id}");
        }
    }

    private string SelectedRowClassFunc(DeclarationDto element, int rowNumber)
    {
        if (selectedRowNumber == rowNumber)
        {
            selectedRowNumber = -1;
            return string.Empty;
        }

        if (_mudDataGrid.SelectedItems.Any(c => c.Id == element.Id))
        {
            selectedRowNumber = rowNumber;
            return "selected";
        }
        
        return string.Empty;
    }

    private async Task<GridData<DeclarationDto>> ServerReload(GridState<DeclarationDto> state)
    {
        var filters = state.FilterDefinitions.Select(c => new FilterRequestDto()
            {
                Field = c.Column?.PropertyName,
                Operator = c.Operator,
                Value = c.Value
            })
            .ToArray();

        var paginate = new PaginatedRequestDto
        {
            Page = state.Page,
            PageSize = state.PageSize,
        };

        var orders = state.SortDefinitions.Select(c => new OrderRequestDto()
            {
                Field = c.SortBy,
                Descending = c.Descending
            })
            .ToArray();

        var result = await Service.SearchAll(paginate, filters, orders);
        
        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            return new GridData<DeclarationDto>
            {
                TotalItems = 0,
                Items = new List<DeclarationDto>()
            };
        }
        
        var items = result.Data!.Items;
        var totalItems = result.Data!.TotalItems;

        return new GridData<DeclarationDto>
        {
            TotalItems = (int) totalItems,
            Items = items
        };
    }
    
    private async Task ExcelExport()
    {
        var filters = _filters.Select(c => new FilterRequestDto()
            {
                Field = c.Column?.PropertyName,
                Operator = c.Operator,
                Value = c.Value
            })
            .ToArray();

        var file = await Service.AllExcelExport(filters, null);

        if (!file.Success)
        {
            ProblemDetailsHandler.Handle(file.Error!);
            return;
        }

        await Js.InvokeVoidAsync("saveAsFile", "Выгрузка.xlsx",
            Convert.ToBase64String(file.Data!));
    }
    
    private async Task ExcelExportAll()
    {
        var file = await Service.AllExcelExportFull();

        if (!file.Success)
        {
            ProblemDetailsHandler.Handle(file.Error!);
            return;
        }

        await Js.InvokeVoidAsync("saveAsFile", "Выгрузка.xlsx",
            Convert.ToBase64String(file.Data!));
    }
}