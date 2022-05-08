using KeyCardWebServices.Data.Models;
using KeyCardWebServices.Extensions;
using KeyCardWebServices.Models.Dtos;
using KeyCardWebServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KeyCardWebServices.Controllers;

[Authorize(AuthenticationSchemes = "WebPortalAuth")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class PunchController : ControllerBase
{
    private readonly IPunchService _punchService;

    public PunchController(IPunchService punchService)
    {
        _punchService = punchService;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "WebPortalAuth,PhysicalAuth")]
    public async Task<IActionResult> Punch()
    {
        var user = await HttpContext.GetUserOrThrow();

        var authGrant = HttpContext.GetAuthGrantType();

        var punch = await _punchService.RegisterPunch(user, Data.Models.Punch.FromAuthGrantType(authGrant));

        return Created($"/api/v1/Punch/{punch.Id}", punch);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPunch([FromRoute] Guid id)
    {
        var user = await HttpContext.GetUserOrThrow();

        var punch = await _punchService.GetPunch(user, id);

        return Ok(punch);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> EditPunch([FromRoute] Guid id, [FromBody] PunchEditDto editDto)
    {
        var user = await HttpContext.GetUserOrThrow();

        await _punchService.EditPunch(user, id, editDto);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePunch([FromRoute] Guid id)
    {
        var user = await HttpContext.GetUserOrThrow();

        await _punchService.DeletePunch(user, id);

        return Ok();
    }

    [HttpPost("history")]
    public async Task<IActionResult> GetPunches([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] PunchFilterDto? filter)
    {
        var user = await HttpContext.GetUserOrThrow();
        
        var punches = await _punchService.GetPunches(user, filter);
        
        return Ok(punches);
    }
}
