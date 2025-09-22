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
    /// <param name="user">Данные пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Статус выполнения операции.</returns>
    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAccountAsync([FromBody] UserDto user, CancellationToken cancellationToken)
    {
        await service.CreateAsync(user, cancellationToken);
        return NoContent();
    }
    
    /// <summary>
    /// Обновление пользователя.
    /// </summary>
    /// <param name="user">Данные пользователя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Статус выполнения операции.</returns>
    [HttpPost("Update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAccountAsync([FromBody] UserDto user, CancellationToken cancellationToken)
    {
        await service.UpdateAsync(user, cancellationToken);
        return NoContent();
    }
}