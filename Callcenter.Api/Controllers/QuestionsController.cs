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
    
    [HttpPost("groups/create")]
    [ProducesResponseType(typeof(QuestionGroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGroup([FromBody] CreateQuestionGroupRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateGroup(request.Name, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPatch("groups/{id:int}/rename")]
    [ProducesResponseType(typeof(QuestionGroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RenameGroup(int id, [FromBody] RenameQuestionGroupRequest request, CancellationToken cancellationToken)
    {
        var result = await service.RenameGroup(id, request.Name, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPatch("{id:int}/answer")]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeQuestionAnswer(int id, [FromBody] ChangeQuestionAnswerRequest request, CancellationToken cancellationToken)
    {
        var result = await service.ChangeQuestionAnswer(id, request.Answer, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPost("create")]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateQuestion([FromBody] QuestionCreateRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateQuestion(request, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteQuestion(int id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteQuestion(id, cancellationToken);
        
        return Ok(result);
    }
}