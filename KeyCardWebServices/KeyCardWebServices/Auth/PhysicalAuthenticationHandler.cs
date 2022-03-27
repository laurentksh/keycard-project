using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace KeyCardWebServices.Auth;

public class PhysicalAuthenticationHandler : AuthenticationHandler<PhysicalAuthenticationOptions>
{
    public PhysicalAuthenticationHandler(IOptionsMonitor<PhysicalAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException();
    }
}

public class PhysicalAuthenticationOptions : AuthenticationSchemeOptions
{

}