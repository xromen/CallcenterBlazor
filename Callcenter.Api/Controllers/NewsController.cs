using Callcenter.Api.Services;
using Callcenter.Shared;
using Callcenter.Shared.Requests;
using Callcenter.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Callcenter.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[Route("[controller]")]
public class NewsController(NewsService service) : ControllerBase
{
    /// <summary>
    /// Создание новости
    /// </summary>
    /// <param name="dto">Информация о новости</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданная новость</returns>
    [HttpPost]
    [ProducesResponseType(typeof(NewsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> Create([FromBody] NewsDto dto, CancellationToken cancellationToken)
    {
        var result = await service.Create(dto, cancellationToken);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Получение новостей с пагинацией
    /// </summary>
    /// <param name="request">Запрос с пагинацией</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список новостей</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<NewsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromQuery] PaginatedRequestDto request, CancellationToken cancellationToken)
    {
        var result = await service.GetNews(request.Page, request.PageSize, cancellationToken);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Обновление новости
    /// </summary>
    /// <param name="id">Идентификатор новости</param>
    /// <param name="dto">Информация и новости</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Измененная новость</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(NewsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> Update(int id, [FromBody] NewsDto dto, CancellationToken cancellationToken)
    {
        var result = await service.Update(id, dto, cancellationToken);
        
        return Ok(result);
    }
    
    /// <summary>
    /// Удаление новости
    /// </summary>
    /// <param name="id">Идентификтор новости</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Удаленная новость</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(NewsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await service.Delete(id, cancellationToken);
        
        return Ok(result);
    }
}