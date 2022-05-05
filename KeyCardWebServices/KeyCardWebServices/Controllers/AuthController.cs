using KeyCardWebServices.Extensions;
using KeyCardWebServices.Models.Dtos;
using KeyCardWebServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyCardWebServices.Controllers;

[Authorize(AuthenticationSchemes = "WebPortalAuth")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var grant = await _authService.Login(loginDto, HttpContext.Connection.RemoteIpAddress ?? System.Net.IPAddress.None);
        
        return Ok(grant);
    }

    [HttpPost("device")]
    public async Task<IActionResult> AuthorizeNewDevice([FromBody] string deviceName)
    {
        var user = await HttpContext.GetUserOrThrow();

        var grant = await _authService.AuthorizeNewDevice(user, Data.Models.AuthGrantType.Physical, deviceName);

        return Ok(grant);
    }
}
