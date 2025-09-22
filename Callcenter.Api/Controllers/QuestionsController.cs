using Callcenter.Api.Services;
using Callcenter.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Callcenter.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[Route("[controller]")]
public class QuestionsController(QuestionsService service) : ControllerBase
{
    [HttpGet("groups")]
    [ProducesResponseType(typeof(List<QuestionGroupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetGroups(CancellationToken cancellationToken)
    {
        var result = await service.GetGroups(cancellationToken);
        
        return Ok(result);
    }
}