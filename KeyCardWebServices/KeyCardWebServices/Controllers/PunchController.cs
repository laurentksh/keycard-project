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
    [HttpPost]
    [Authorize(AuthenticationSchemes = "WebPortalAuth,PhysicalAuth")]
    public async Task<IActionResult> Punch()
    {

        return Ok();
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetPunches()
    {

        return Ok();
    }
}
