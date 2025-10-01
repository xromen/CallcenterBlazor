using Callcenter.Shared;
using Callcenter.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Components.Dialogs;

public partial class IdentPersonDialog : ComponentBase
{
    [Parameter] public string? FirstName { get; set; }

    [Parameter] public string? SecName { get; set; }

    [Parameter] public string? FathName { get; set; }

    [Parameter] public DateTime? BirthDate { get; set; }

    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }

    [Inject] private DeclarationsService DeclarationsService { get; set; }

    [Inject] private ProblemDetailsHandler ProblemDetailsHandler { get; set; }
    
    [Inject] private ISnackbar Snackbar { get; set; }

    private void Cancel() => MudDialog.Cancel();

    private int? _selectedRowNumber;
    private IdentedPersonDto? _selectedPerson;
    private MudTable<IdentedPersonDto> mudTable;

    private string SelectedRowClassFunc(IdentedPersonDto person, int rowNumber)
    {
        if (_selectedRowNumber == rowNumber)
        {
            _selectedRowNumber = null;
            mudTable.SelectedItem = null;
            return string.Empty;
        }
        else if (mudTable.SelectedItem != null && mudTable.SelectedItem.Equals(person))
        {
            _selectedRowNumber = rowNumber;
            return "selected";
        }
        else
        {
            return string.Empty;
        }
    }

    private void RowClickEvent(TableRowClickEventArgs<IdentedPersonDto> args)
    {

    }

    private async Task<TableData<IdentedPersonDto>> ServerReload(TableState state, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(SecName) || !BirthDate.HasValue)
        {
            Snackbar.Add("Не указаны обязательные параметры", Severity.Error);
            return new() { Items = [], TotalItems = 0 };
        }
        
        var result = await DeclarationsService.IdentPerson(SecName, FirstName, FathName, BirthDate.Value, cancellationToken);

        if (!result.Success)
        {
            ProblemDetailsHandler.Handle(result.Error!);
            return new() { Items = [], TotalItems = 0 };
        }

        return new TableData<IdentedPersonDto>()
        {
            Items = result.Data!,
            TotalItems = result.Data!.Count,
        };
    }

    private void Choose(MouseEventArgs obj)
    {
        MudDialog.Close(DialogResult.Ok(_selectedPerson));
    }
}