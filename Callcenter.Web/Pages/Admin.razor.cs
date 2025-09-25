using Bogus;
using Callcenter.Web.Components;
using Callcenter.Web.Components.Dialogs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Callcenter.Web.Pages;

public partial class Admin : ComponentBase
{
    [Inject] private IDialogService Dialog { get; set; } = null!;
    private List<NewsDto> _news = new();
    private bool _isLoading = false;
    
    private List<UserModel> _users = new();
    private List<string> _orgs = new();

    public static List<string> WorkGroups =
    [
        "администратор кц",
        "инженер кц",
        "оператор 1 уровня оккмп",
        "оператор 1 уровня оомс",
    ];

    protected override async Task OnInitializedAsync()
    {
        await NewsLoad();
    }

    private async Task TabIndexChanged(int arg)
    {
        _isLoading = true;
        
        switch (arg)
        {
            //Новости
            case 0:
                await NewsLoad();
                break;
            //Пользователи
            case 1:
                await UsersLoad();
                break;
        }
        
        _isLoading = false;
    }

    private async Task NewsLoad()
    {
        _news = News.NewsDtos;
    }

    private async Task UsersLoad()
    {
        var personFaker = new Faker<UserModel>()
            .RuleFor(p => p.Id, f => f.IndexFaker)
            .RuleFor(p => p.Fullname, f => f.Name.FullName() + " " + f.Name.FirstName())
            .RuleFor(p => p.Username,f => f.Name.FirstName())
            .RuleFor(p => p.Organisation,f => f.PickRandom(News.Orgs))
            .RuleFor(p => p.WorkGroup,f => f.PickRandom(WorkGroups))
            .RuleFor(p => p.SpLevel,f => f.Random.Int(1, 2).ToString())
            .RuleFor(p => p.IsActive,f => f.Random.Bool());

        _users = personFaker.Generate(100);
        _orgs = _users.Select(u => u.Organisation).Distinct().ToList();
    }

    private async Task NewsRowClicked(NewsClickEventArgs arg)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Medium, FullWidth = true };
        var parameters = new DialogParameters<NewsEditDialog> { { x => x.NewsObj, arg.Item } };
        
        await Dialog.ShowAsync<NewsEditDialog>("Custom Options Dialog", parameters, options);
    }

    private async Task AddNews(MouseEventArgs arg)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Medium, FullWidth = true };
        
        await Dialog.ShowAsync<NewsEditDialog>("Custom Options Dialog", options);
    }

    private async Task UserCardClick(MouseEventArgs arg, UserModel user)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Small, FullWidth = true };
        var parameters = new DialogParameters<UserEditDialog> { { x => x.User, user} };
        
        await Dialog.ShowAsync<UserEditDialog>("Custom Options Dialog", parameters, options);
    }

    private async Task AddUser(MouseEventArgs arg)
    {
        DialogOptions options = new() { MaxWidth = MaxWidth.Small, FullWidth = true };
        
        await Dialog.ShowAsync<UserEditDialog>("Custom Options Dialog", options);
    }
}

public class UserModel
{
    public int? Id { get; set; }
    public string Fullname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Organisation { get; set; }
    public string WorkGroup { get; set; }
    public string SpLevel { get; set; }
    public bool IsActive { get; set; }
}