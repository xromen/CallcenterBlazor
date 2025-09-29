using Callcenter.Api.Data.Entities;
using Callcenter.Api.Services;
using Callcenter.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Callcenter.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = "admin")]
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAccountAsync([FromBody] UserCreateDto userCreate, CancellationToken cancellationToken)
    {
        await service.CreateAsync(userCreate, cancellationToken);
        return NoContent();
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAccountsAsync(CancellationToken cancellationToken)
    {
        var users = await service.GetUsersAsync(cancellationToken);
        
        return Ok(users);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAccountByIdAsync(int id, CancellationToken cancellationToken)
    {
        var users = await service.GetUserByIdAsync(id, cancellationToken);
        
        return Ok(users);
    }
    
    [HttpGet("groups")]
    [ProducesResponseType(typeof(List<UserGroupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserGroupsAsync(CancellationToken cancellationToken)
    {
        var users = await service.GetUserGroupsAsync(cancellationToken);
        
        return Ok(users);
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAccountAsync(int id, CancellationToken cancellationToken)
    {
        var users = await service.DeleteUserAsync(id, cancellationToken);
        
        return Ok(users);
    }
    
    /// <summary>
    /// Обновление пользователя.
    /// </summary>
    /// <param name="userCreate">Данные пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Статус выполнения операции.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAccountAsync(int id, [FromBody] UserCreateDto userCreate, CancellationToken cancellationToken)
    {
        await service.UpdateAsync(userCreate, cancellationToken);
        return NoContent();
    }
}