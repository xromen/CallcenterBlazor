using Callcenter.Api.Data.Entities;
using Callcenter.Api.Services;
using Callcenter.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Callcenter.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[Route("[controller]")]
public class AccountsController(AccountsService service) : ControllerBase
{
    /// <summary>
    /// Создание нового пользователя.
    /// </summary>
    /// <param name="userCreate">Данные пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Статус выполнения операции.</returns>
    [HttpPost("Create")]
    [Authorize(Policy = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAccountAsync([FromBody] UserCreateDto userCreate, CancellationToken cancellationToken)
    {
        await service.CreateAsync(userCreate, cancellationToken);
        return NoContent();
    }
    
    /// <summary>
    /// Возвращает список всех пользователей приложения
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список пользователей</returns>
    [HttpGet]
    [Authorize(Policy = "admin")]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAccountsAsync(CancellationToken cancellationToken)
    {
        var users = await service.GetUsersAsync(cancellationToken);
        
        return Ok(users);
    }
    
    /// <summary>
    /// Получение пользователя по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Информация о пользователе</returns>
    [HttpGet("{id:int}")]
    [Authorize(Policy = "admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAccountByIdAsync(int id, CancellationToken cancellationToken)
    {
        var users = await service.GetUserByIdAsync(id, cancellationToken);
        
        return Ok(users);
    }
    
    /// <summary>
    /// Получение групп/ролей пользователей из справочника
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список групп</returns>
    [HttpGet("groups")]
    [Authorize(Policy = "admin")]
    [ProducesResponseType(typeof(List<UserGroupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserGroupsAsync(CancellationToken cancellationToken)
    {
        var users = await service.GetUserGroupsAsync(cancellationToken);
        
        return Ok(users);
    }
    
    /// <summary>
    /// Получение не просмотренных уведомлений текущего пользователя (кешируется 5 минут)
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список уведомлений</returns>
    [HttpGet("notifications")]
    [ProducesResponseType(typeof(List<UserNotificationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ResponseCache(Duration = 60 * 5, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> GetUserNotificationsAsync(CancellationToken cancellationToken)
    {
        var notifications = await service.GetUserNotificationsAsync(cancellationToken);
        
        return Ok(notifications);
    }
    
    /// <summary>
    /// Просмотр уведомления
    /// </summary>
    /// <param name="id">Идентификтор уведомления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    [HttpGet("notifications/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReadNotificationAsync(int id, CancellationToken cancellationToken)
    {
        await service.ReadNotificationAsync(id, cancellationToken);
        
        return Ok();
    }
    
    /// <summary>
    /// Удаление аккаунта пользователя
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Информация об удаленном пользователе</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAccountAsync(int id, CancellationToken cancellationToken)
    {
        var users = await service.DeleteUserAsync(id, cancellationToken);
        
        return Ok(users);
    }
    
    /// <summary>
    /// Обновление пользователя
    /// </summary>
    /// <param name="userCreate">Данные пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Статус выполнения операции</returns>
    [HttpPut("{id:int}")]
    [Authorize(Policy = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAccountAsync(int id, [FromBody] UserCreateDto userCreate, CancellationToken cancellationToken)
    {
        await service.UpdateAsync(userCreate, cancellationToken);
        return NoContent();
    }
}