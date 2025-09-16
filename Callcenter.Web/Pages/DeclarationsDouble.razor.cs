using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Callcenter.Web.Pages;

public partial class DeclarationsDouble : ComponentBase
{
    private List<DeclarationDoubleModel> _declarations =
    [
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
        new("Макисм", "Казеев", "Александрович", new(2000, 11, 27), 10),
    ];

    private async Task<GridData<DeclarationDoubleModel>> ServerReload(GridState<DeclarationDoubleModel> state)
    {
        return new()
        {
            Items = _declarations.Skip(state.PageSize * state.Page).Take(state.PageSize).ToList(),
            TotalItems = _declarations.Count(),
        };
    }
}

public class DeclarationDoubleModel
{
    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string SurName { get; set; }

    /// <summary>
    /// Отчество 
    /// </summary>
    public string LastName { get; set; }

    public DateOnly BirthDate { get; set; }

    public int DoubleCount { get; set; }

    public DeclarationDoubleModel(string firstName, string surName, string lastName, DateOnly birthDate,
        int doubleCount)
    {
        FirstName = firstName;
        SurName = surName;
        LastName = lastName;
        BirthDate = birthDate;
        DoubleCount = doubleCount;
    }
}