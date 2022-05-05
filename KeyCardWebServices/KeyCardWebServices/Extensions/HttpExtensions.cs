using System.Net;
using System.Security.Claims;
using KeyCardWebServices.Data.Models;
using KeyCardWebServices.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KeyCardWebServices.Extensions;

public static class HttpExtensions
{
    /// <summary>
    /// Returns whether the Request is authenticated or not.
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <returns>true: Authenticated; false: Not Authenticated</returns>
    public static bool IsAuthenticated(HttpContext context) => context.User.Identity.IsAuthenticated;

    /// <summary>
    /// Get the user from HttpContext.
    /// </summary>
    /// <remarks>
    /// WARNING: The returned AppUser object does not contains foreign properties.
    /// </remarks>
    /// <param name="context">Request's HttpContext</param>
    /// <returns>AppUser or null</returns>
    public static async Task<AppUser?> GetUser(this HttpContext context)
    {
        var userManager = context.RequestServices.GetRequiredService(typeof(UserManager<AppUser>)) as UserManager<AppUser>;

        var claim = context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        
        if (claim == null)
            return null;

        var userId = claim.Value;

        if (userId == null)
            return null;

        var user = await userManager!.FindByIdAsync(userId);

        return user;
    }

    public static async Task<AppUser> GetUserOrThrow(this HttpContext context) =>
        await GetUser(context) ?? throw new HttpException(HttpStatusCode.Unauthorized, "Could not authenticate user");

    /// <summary>
    /// Get the user's id from HttpContext.
    /// </summary>
    /// <param name="context">Request's HttpContext</param>
    /// <returns>User's id or <see cref="Guid.Empty"/></returns>
    public static Guid GetUserId(this HttpContext context)
    {
        if (IsAuthenticated(context))
        {
            var id = context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return Guid.Parse(id ?? throw new Exception("Could not get user id from token"));
        }
        else
        {
            return Guid.Empty;
        }
    }

    public static IActionResult Handle(this HttpRequest request, Exception exception)
    {
        var (httpStatus, problemType, problemMsg) = GetProblem(exception);

        var problemDetails = new ProblemDetails
        {
            Type = $"/Problems/{problemType}",
            Title = httpStatus.ToString(),
            Status = (int)httpStatus,
            Detail = problemMsg,
            Instance = request.Path
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    private static (HttpStatusCode httpStatus, string problemType, string problemMsg) GetProblem(Exception exception)
    {
        var exMsg = exception.Message ?? "Unspecified error message";

        return exception switch
        {
            HttpException httpEx => (httpEx.HttpStatusCode, httpEx.HttpStatusCode.ToString(), exMsg),
            _ => (HttpStatusCode.InternalServerError, HttpStatusCode.InternalServerError.ToString(), exMsg)
        };
    }
}
