using System.Security.Claims;
using System.Text.Encodings.Web;
using KeyCardWebServices.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace KeyCardWebServices.Auth;

public class PhysicalAuthenticationHandler : AuthenticationHandler<PhysicalAuthenticationOptions>
{
    private readonly IAuthService _authService;

    public PhysicalAuthenticationHandler(
        IAuthService authService,
        IOptionsMonitor<PhysicalAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _authService = authService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Context.Request.Headers.TryGetValue("x-physicalauth", out var header))
            return AuthenticateResult.NoResult();

        var result = await _authService.AuthenticateUsingPhysicalKey(header.ToString());

        if (result == null)
            return AuthenticateResult.Fail("INVALID_KEY");
        else
        {
            if (result.ExpirationDate < Clock.UtcNow)
                return AuthenticateResult.Fail("GRANT_EXPIRED");

            var id = new System.Security.Principal.GenericIdentity(result.IssuedTo.Email, "physical");
            id.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.IssuedToId.ToString()));
            id.AddClaim(new Claim("jti", result.Id.ToString()));
            id.AddClaim(new Claim(ClaimTypes.Expiration, result.ExpirationDate.ToString()));

            return AuthenticateResult.Success(
                new AuthenticationTicket(new ClaimsPrincipal(id), Scheme.Name)
                );
        }
    }
}

public class PhysicalAuthenticationOptions : AuthenticationSchemeOptions
{
    
}