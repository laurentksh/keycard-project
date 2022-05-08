using System.IdentityModel.Tokens.Jwt;
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
    public async Task<IActionResult> AuthorizeNewDevice([FromBody] GrantPhysicalDeviceDto device)
    {
        var user = await HttpContext.GetUserOrThrow();

        var grant = await _authService.AuthorizeNewDevice(user, Data.Models.AuthGrantType.DeviceKey, device.DeviceName);

        return Ok(grant);
    }

    [Authorize(AuthenticationSchemes = "WebPortalAuth,PhysicalAuth")]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var user = await HttpContext.GetUserOrThrow();

        await _authService.InvalidateGrant(user, Guid.Parse(User.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value));

        return Ok();
    }
}
