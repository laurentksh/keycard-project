using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using KeyCardWebServices.Data;
using KeyCardWebServices.Data.Models;
using KeyCardWebServices.Exceptions;
using KeyCardWebServices.Models.Dtos;
using KeyCardWebServices.Models.ViewModels;
using KeyCardWebServices.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KeyCardWebServices.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthService(
        ApplicationDbContext context,
        IConfiguration configuration,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
    {
        _dbContext = context;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<AuthGrant?> AuthenticateUsingPhysicalKey(string physicalKey)
    {
        return await _dbContext.AuthGrants.SingleOrDefaultAsync(x => x.Token == physicalKey && x.Type == AuthGrantType.Physical);
    }

    public async Task<AuthGrantViewModel> Login(LoginDto loginDto, IPAddress remote)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
            throw new HttpException(HttpStatusCode.BadRequest, "User does not exist");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);

        if (result.Succeeded)
        {
            var grant = await AuthorizeNewDevice(user, AuthGrantType.Jwt, $"Web - {remote}");

            return grant;
        }
        else
        {
            if (result.IsNotAllowed)
                throw new HttpException(HttpStatusCode.Forbidden, "Login disallowed");
            if (result.IsLockedOut)
                throw new HttpException(HttpStatusCode.TooManyRequests, "Too many requests, please try again later");
            if (result.RequiresTwoFactor)
                throw new HttpException(HttpStatusCode.Unauthorized, "MFA required");

            throw new HttpException(HttpStatusCode.Unauthorized, "Username and password do not match");
        }
    }

    public async Task<AuthGrantViewModel> AuthorizeNewDevice(AppUser user, AuthGrantType authGrantType, string deviceName)
    {
        AuthGrant grant;

        if (authGrantType == AuthGrantType.Jwt)
        {
            var token = BuildJwt(user, await _userManager.GetClaimsAsync(user), await _userManager.GetRolesAsync(user));

            grant = new AuthGrant
            {
                Id = Guid.Parse(token.Id),
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Device = deviceName,
                Type = authGrantType,
                IssuedTo = user,
                IssuedToId = user.Id,
                CreationDate = token.IssuedAt,
                ExpirationDate = token.ValidTo
            };
        }
        else if (authGrantType == AuthGrantType.Physical)
        {
            grant = new AuthGrant
            {
                Id = Guid.NewGuid(),
                Token = StringUtilities.SecureRandom(32, StringUtilities.AllowedChars.All),
                Device = deviceName,
                Type = authGrantType,
                IssuedTo = user,
                IssuedToId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddYears(1),
            };
        }
        else
            throw new NotSupportedException("Unsupported AuthGrantType");

        await _dbContext.AuthGrants.AddAsync(grant);
        await _dbContext.SaveChangesAsync();

        return AuthGrantViewModel.FromAuthGrant(grant);
    }

    public async Task<List<AuthGrantViewModel>> GetActiveGrants(AppUser user)
    {
        return await _dbContext.AuthGrants
            .Where(x => x.IssuedToId == user.Id)
            .Select(x => AuthGrantViewModel.FromAuthGrant(x))
            .ToListAsync();
    }

    public async Task InvalidateGrant(AppUser user, Guid grantId)
    {
        var grant = await _dbContext.AuthGrants.SingleOrDefaultAsync(x => x.Id == grantId);

        if (grant == null)
            throw new HttpException(HttpStatusCode.NotFound);

        if (grant.IssuedToId != user.Id)
            throw new HttpException(HttpStatusCode.Forbidden);

        _dbContext.AuthGrants.Remove(grant);
        await _dbContext.SaveChangesAsync();
    }

    public async Task InvalidateAllGrants(AppUser user)
    {
        var grants = _dbContext.AuthGrants.Where(x => x.IssuedToId == user.Id);
        _dbContext.AuthGrants.RemoveRange(grants);
        await _dbContext.SaveChangesAsync();
    }

    public JwtSecurityToken BuildJwt(AppUser appUser, IEnumerable<Claim>? additionalClaims = null, IEnumerable<string>? additionalRoles = null)
    {
        var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtSigningKey"));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        if (additionalClaims != null)
            claims.AddRange(additionalClaims);

        if (additionalRoles != null)
            claims.AddRange(additionalRoles.Select(x => new Claim(ClaimTypes.Role, x)));

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMonths(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            );

        return token;
    }
}
