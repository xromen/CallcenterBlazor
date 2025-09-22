using Callcenter.Api.Services;
using Callcenter.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Callcenter.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[Route("[controller]")]
//[ResponseCache(Duration = 3600 * 8, Location = ResponseCacheLocation.Any, NoStore = false)]
public class DictionariesController(DictionariesService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<DictionariesDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAnswerStatuses(CancellationToken cancellationToken)
    {
        var result = await service.GetDictionaries(cancellationToken);
        
        return Ok(result);
    }
}