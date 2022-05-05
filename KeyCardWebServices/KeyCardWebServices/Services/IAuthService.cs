using System.Net;
using KeyCardWebServices.Data;
using KeyCardWebServices.Data.Models;
using KeyCardWebServices.Models.Dtos;
using KeyCardWebServices.Models.ViewModels;

namespace KeyCardWebServices.Services;

public interface IAuthService
{
    /// <summary>
    /// Authenticates with a physical key.
    /// </summary>
    /// <param name="physicalKey"></param>
    /// <returns>The grant if successfull, or null if failed.</returns>
    Task<AuthGrant?> AuthenticateUsingPhysicalKey(string physicalKey);

    Task<AuthGrantViewModel> Login(LoginDto loginDto, IPAddress remote);

    /// <summary>
    /// Authorize a new device.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="authGrantType"></param>
    /// <param name="deviceName"></param>
    /// <returns></returns>
    Task<AuthGrantViewModel> AuthorizeNewDevice(AppUser user, AuthGrantType authGrantType, string deviceName);

    /// <summary>
    /// Get active grants.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<List<AuthGrantViewModel>> GetActiveGrants(AppUser user);

    /// <summary>
    /// Invalidate a specific grant.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="grantId"></param>
    /// <returns></returns>
    Task InvalidateGrant(AppUser user, Guid grantId);

    /// <summary>
    /// Invalidates all active grants.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task InvalidateAllGrants(AppUser user);
}
