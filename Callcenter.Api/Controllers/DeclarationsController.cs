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
public class DeclarationsController(DeclarationService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(GetDeclarationsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDeclarations([FromQuery] PaginatedRequestDto request, CancellationToken cancellationToken)
    {
        var result = await service.GetDeclarations(request.Page, request.PageSize, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("{id:int}/history")]
    [ProducesResponseType(typeof(List<DeclarationActionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDeclarationHistory(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetDeclarationHistory(id, cancellationToken);
        
        if(result == null)
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetDeclarationsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDeclaration(int id, CancellationToken cancellationToken)
    {
        var result = await service.GetDeclarationById(id, cancellationToken);
        
        if(result == null)
            return NotFound();
        
        return Ok(result);
    }
}