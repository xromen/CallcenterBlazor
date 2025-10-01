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
    public async Task CreateAsync(UserCreateDto userCreate, CancellationToken cancellationToken = default)
    {
        if (UserNameRegex().IsMatch(userCreate.Login) == false)
        {
            throw new IncorrectDataException("Извините, но имя пользователя не может содержать служебные символы.");
        }

        var userExisted = await userManager.FindByNameAsync(userCreate.Login);

        if (userExisted != null)
        {
            throw new IncorrectDataException("Извините, но пользователь с таким именем уже зарегистрирован. Пожалуйста, попробуйте другое имя пользователя.");
        }

        var appUser = userCreate.Adapt<User>();

        appUser.ParentUserId = environment.AuthUser.Id;
        appUser.DateAdded = DateTime.Now;

        var result = await userManager.CreateAsync(appUser, userCreate.Password);

        if (result.Succeeded == false)
        {
            throw new IncorrectDataException($"Ошибки: {string.Join("; ", result.Errors.Select(error => error.Description))}");
        }
    }
    
    public async Task UpdateAsync(UserCreateDto userCreate, CancellationToken cancellationToken = default)
    {
        var appUser = await userManager.FindByIdAsync(userCreate.Id.ToString());

        if (appUser == null)
        {
            throw new EntityNotFoundException("Пользователь с указанным Id не найден");
        }
        
        var passwordHasher = userManager.PasswordHasher;

        if (!string.IsNullOrWhiteSpace(userCreate.Login))
            appUser.Login = userCreate.Login;

        if (!string.IsNullOrWhiteSpace(userCreate.FullName))
            appUser.FullName = userCreate.FullName;
        
        if(userCreate.GroupId.HasValue)
            appUser.GroupId = userCreate.GroupId.Value;
        
        appUser.IsEnabled = userCreate.IsEnabled;
        appUser.SpLevel = userCreate.SpLevel;

        if (!string.IsNullOrWhiteSpace(userCreate.Password))
        {
            appUser.Password = passwordHasher.HashPassword(appUser, userCreate.Password);
        }
        
        var result = await userManager.UpdateAsync(appUser);

        if (result.Succeeded == false)
        {
            throw new IncorrectDataException($"Ошибки: {string.Join("; ", result.Errors.Select(error => error.Description))}");
        }
    }

    public async Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var users = await context.Users
            .Include(c => c.Organisation)
            .Include(c => c.Group)
            .Where(c => c.IsEnabled)
            .ToListAsync(cancellationToken);
        
        return users.Adapt<List<UserDto>>();
    }

    public async Task<UserDto> DeleteUserAsync(int id, CancellationToken cancellationToken)
    {
        var dbUser = await userManager.FindByIdAsync(id.ToString());

        if (dbUser == null)
        {
            throw new EntityNotFoundException();
        }
        
        await userManager.DeleteAsync(dbUser);
        
        return dbUser.Adapt<UserDto>();
    }

    public async Task<UserDto> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            throw new EntityNotFoundException();
        }
        
        return user.Adapt<UserDto>();
    }

    public async Task<List<UserGroupDto>> GetUserGroupsAsync(CancellationToken cancellationToken)
    {
        var groups = await context.UserGroups.ToListAsync(cancellationToken);
        
        return groups.Adapt<List<UserGroupDto>>();
    }
    
    [GeneratedRegex("^[a-zA-Z0-9_-]+$", RegexOptions.Compiled)]
    private static partial Regex UserNameRegex();

    public async Task<List<UserNotificationDto>> GetUserNotificationsAsync(CancellationToken cancellationToken)
    {
        if (environment.AuthUser == null)
        {
            throw new PermissionException("Не авторизованный доступ");
        }
        
        var notifications = await context.UserNotifications
            .Include(n => n.NotificationType)
            .Include(n => n.User)
            .Include(n => n.UserWhoSend)
            .Where(c => c.IsRead == false && c.UserId == environment.AuthUser.Id)
            .ToListAsync(cancellationToken);
        
        return notifications.Adapt<List<UserNotificationDto>>();
    }

    public async Task ReadNotificationAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await context.UserNotifications.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException();
        }
        
        entity.IsRead = true;
        
        await context.SaveChangesAsync(cancellationToken);
    }
}