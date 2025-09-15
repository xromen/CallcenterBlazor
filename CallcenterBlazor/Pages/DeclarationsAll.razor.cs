using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CallcenterBlazor.Pages;

public partial class DeclarationsAll : ComponentBase
{
    private static Random _rnd = new Random();
    private int selectedRowNumber = -1;
    private MudDataGrid<DeclarationDetail> _mudDataGrid;

    private List<DeclarationDetail> _declarations =
    [
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
        new()
        {
            Operator = "Попов Денис Алексанрович", Number = "ХКФОМС-" + _rnd.Next(100),
            EjogNumber = _rnd.Next(100000).ToString(),
            Status = "Отправлен ответ", Code = "ХКФОМС"
        },
    ];

    protected override void OnInitialized()
    {
        for (int i = 0; i < _declarations.Count; i++)
        {
            _declarations[i].Id = i;
        }
    }
    private void RowClickEvent(DataGridRowClickEventArgs<DeclarationDetail> tableRowClickEventArgs)
    {
    }

    private string SelectedRowClassFunc(DeclarationDetail element, int rowNumber)
    {
        return _mudDataGrid.SelectedItems.Contains(element) ? "selected" : "";
    }

    private async Task<GridData<DeclarationDetail>> ServerReload(GridState<DeclarationDetail> state)
    {
        IEnumerable<DeclarationDetail> data = _declarations;
        await Task.Delay(300);
        data = data.Where(item =>
        {
            var result = true;
            foreach (var filter in state.FilterDefinitions)
            {
                if (filter.Value == null)
                {
                    continue;
                }
                
                switch (filter.Column.PropertyName)
                {
                    case nameof(DeclarationDetail.Number):
                        if (!item.Number.Contains(filter.Value.ToString()))
                            return false;
                        break;
                }
            }

            return true;
        }).ToArray();
        var totalItems = data.Count();

        var sortDefinition = state.SortDefinitions.FirstOrDefault();
        if (sortDefinition != null)
        {
            switch (sortDefinition.SortBy)
            {
                case nameof(DeclarationDetail.Number):
                    data = data.OrderByDirection(
                        sortDefinition.Descending ? SortDirection.Descending : SortDirection.Ascending,
                        o => o.Number
                    );
                    break;
                case nameof(DeclarationDetail.Operator):
                    data = data.OrderByDirection(
                        sortDefinition.Descending ? SortDirection.Descending : SortDirection.Ascending,
                        o => o.Operator
                    );
                    break;
                case nameof(DeclarationDetail.Status):
                    data = data.OrderByDirection(
                        sortDefinition.Descending ? SortDirection.Descending : SortDirection.Ascending,
                        o => o.Status
                    );
                    break;
                case nameof(DeclarationDetail.Code):
                    data = data.OrderByDirection(
                        sortDefinition.Descending ? SortDirection.Descending : SortDirection.Ascending,
                        o => o.Code
                    );
                    break;
                case nameof(DeclarationDetail.EjogNumber):
                    data = data.OrderByDirection(
                        sortDefinition.Descending ? SortDirection.Descending : SortDirection.Ascending,
                        o => o.EjogNumber
                    );
                    break;
                default:
                    var sortByColumn = _mudDataGrid.RenderedColumns.First(c => c.PropertyName == sortDefinition.SortBy);
                    switch (sortByColumn.Title)
                    {
                        case nameof(DeclarationDetail.Operator):
                            data = data.OrderByDirection(
                                sortDefinition.Descending ? SortDirection.Descending : SortDirection.Ascending,
                                o => o.Operator
                            );
                            break;
                    }

                    break;
            }
        }

        var pagedData = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
        return new GridData<DeclarationDetail>
        {
            TotalItems = totalItems,
            Items = pagedData
        };
    }
}

public class DeclarationDetail
{
    public long Id { get; set; }
    public string Operator { get; set; }
    public string Number { get; set; }
    public string EjogNumber { get; set; }
    public string Status { get; set; }
    public string Code { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is DeclarationDetail item)
        {
            return Id == item.Id;
        }
        
        return false;
    }
}