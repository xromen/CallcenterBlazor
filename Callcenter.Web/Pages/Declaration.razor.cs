using Bogus;
using Microsoft.AspNetCore.Components;

namespace Callcenter.Web.Pages;

public partial class Declaration : ComponentBase
{
    [Parameter] public long? Id { get; set; }

    private List<RkAction> _actions = new();

    private string? _svoStatus = null;

    private static string[] _actionStrings =
    [
        "Создание РК",
        "Прикрепление документа",
        "Редактирование РК",
        "Удаление документа",
    ];

    protected override void OnInitialized()
    {
        var personFaker = new Faker<RkAction>()
            .RuleFor(p => p.Date, f => f.Date.Recent())
            .RuleFor(p => p.Action,  f => f.PickRandom(_actionStrings))
            .RuleFor(p => p.User,       f => "администратор кц Попов Денис Александрович");

        _actions = personFaker.Generate(10);

        _actions = _actions.OrderByDescending(c => c.Date).ToList();
    }
}

public class RkAction
{
    public DateTime Date { get; set; }
    public string Action { get; set; }
    public string User { get; set; }
}