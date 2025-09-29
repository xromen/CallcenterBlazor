using Callcenter.Api.Services;
using Callcenter.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Callcenter.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = "admin")]
[Route("[controller]")]
public class ReportsController(ReportsService service) : ControllerBase
{
    [HttpGet("PgFormGeneral")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPgFormGeneral([FromQuery] FormRequestDto request, CancellationToken cancellationToken)
    {
        var report = await service.GetPgFormGeneral(request.From, request.To, cancellationToken);
        
        return File(report, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ПГ общая.xlsx");
    }
    
    [HttpGet("PgFormGroupByMo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPgFormGroupByMo([FromQuery] FormRequestDto request, CancellationToken cancellationToken)
    {
        var report = await service.GetPgFormGroupByMo(request.From, request.To, cancellationToken);
        
        return File(report, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ПГ в разрезе МО.xlsx");
    }
    
    [HttpGet("OmsPerformanceCriteriaForm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOmsPerformanceCriteriaForm([FromQuery] int month, CancellationToken cancellationToken)
    {
        var report = await service.GetOmsPerformanceCriteriaForm(month, cancellationToken);
        
        return File(report, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ПГ в разрезе МО.xlsx");
    }
    
    [HttpGet("JustifiedComplaintsForm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetJustifiedComplaintsForm([FromQuery] FormRequestDto request, CancellationToken cancellationToken)
    {
        var report = await service.GetJustifiedComplaintsForm(request.From, request.To, cancellationToken);
        
        return File(report, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ПГ в разрезе МО.xlsx");
    }
    
    [HttpGet("AllComplaintsForm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllComplaintsForm([FromQuery] FormRequestDto request, CancellationToken cancellationToken)
    {
        var report = await service.GetAllComplaintsForm(request.From, request.To, cancellationToken);
        
        return File(report, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ПГ в разрезе МО.xlsx");
    }
}