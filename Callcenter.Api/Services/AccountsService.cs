using System.Text.RegularExpressions;
using Callcenter.Api.Data;
using Callcenter.Api.Data.Entities;
using Callcenter.Api.Models;
using Callcenter.Api.Models.Exceptions;
using Callcenter.Shared;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Callcenter.Api.Services;

public partial class AccountsService(
    RequestEnvironment environment,
    UserManager<User> userManager,
    ApplicationDbContext context)
{
    public async Task CreateAsync(UserDto user, CancellationToken cancellationToken = default)
    {
        if (UserNameRegex().IsMatch(user.Login) == false)
        {
            throw new IncorrectDataException("Извините, но имя пользователя не может содержать служебные символы.");
        }

        var userExisted = await userManager.FindByNameAsync(user.Login);

        if (userExisted != null)
        {
            throw new IncorrectDataException("Извините, но пользователь с таким именем уже зарегистрирован. Пожалуйста, попробуйте другое имя пользователя.");
        }

        var appUser = user.Adapt<User>();

        appUser.ParentUserId = environment.AuthUser.Id;
        appUser.DateAdded = DateTime.Now;

        var result = await userManager.CreateAsync(appUser, user.Password);

        if (result.Succeeded == false)
        {
            throw new IncorrectDataException($"Ошибки: {string.Join("; ", result.Errors.Select(error => error.Description))}");
        }
    }
    
    public async Task UpdateAsync(UserDto user, CancellationToken cancellationToken = default)
    {
        var appUser = await userManager.FindByIdAsync(user.Id.ToString());

        if (appUser == null)
        {
            throw new IncorrectDataException("Пользователь с указанным Id не найден");
        }
        
        var passwordHasher = userManager.PasswordHasher;
        
        appUser.Login = user.Login;
        appUser.FullName = user.FullName;
        appUser.GroupId = user.GroupId;
        appUser.IsEnabled = user.IsEnabled;
        appUser.SpLevel = user.SpLevel;

        if (!string.IsNullOrWhiteSpace(user.Password))
        {
            appUser.Password = passwordHasher.HashPassword(appUser, user.Password);
        }
        
        var result = await userManager.UpdateAsync(appUser);

        if (result.Succeeded == false)
        {
            throw new IncorrectDataException($"Ошибки: {string.Join("; ", result.Errors.Select(error => error.Description))}");
        }
    }
    
    [GeneratedRegex("^[a-zA-Z0-9_-]+$", RegexOptions.Compiled)]
    private static partial Regex UserNameRegex();
}