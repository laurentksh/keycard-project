using KeyCardWebServices.Extensions;
using KeyCardWebServices.Models.Dtos;
using KeyCardWebServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        var punch = await _punchService.RegisterPunch(user);

        return Created($"/{punch.Id}", punch);
    }

    [HttpGet("/{id}")]
    public async Task<IActionResult> GetPunch([FromRoute] Guid id)
    {
        var user = await HttpContext.GetUserOrThrow();

        var punch = await _punchService.GetPunch(user, id);

        return Ok(punch);
    }

    [HttpPatch("/{id}")]
    public async Task<IActionResult> EditPunch([FromRoute] Guid id, [FromBody] PunchEditDto editDto)
    {
        var user = await HttpContext.GetUserOrThrow();

        await _punchService.EditPunch(user, editDto);

        return Ok();
    }

    [HttpDelete("/{id}")]
    public async Task<IActionResult> DeletePunch([FromRoute] Guid id)
    {
        var user = await HttpContext.GetUserOrThrow();

        await _punchService.DeletePunch(user, id);

        return Ok();
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetPunches([FromBody] PunchFilterDto filter)
    {
        var user = await HttpContext.GetUserOrThrow();
        
        var punches = await _punchService.GetPunches(user, filter);
        
        return Ok(punches);
    }
}
