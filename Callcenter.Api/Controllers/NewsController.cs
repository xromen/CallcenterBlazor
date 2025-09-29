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
    [HttpPost]
    [ProducesResponseType(typeof(NewsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> Create([FromBody] NewsDto dto, CancellationToken cancellationToken)
    {
        var result = await service.Create(dto, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<NewsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromQuery] PaginatedRequestDto request, CancellationToken cancellationToken)
    {
        var result = await service.GetNews(request.Page, request.PageSize, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(NewsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> Update(int id, [FromBody] NewsDto dto, CancellationToken cancellationToken)
    {
        var result = await service.Update(id, dto, cancellationToken);
        
        return Ok(result);
    }
    
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