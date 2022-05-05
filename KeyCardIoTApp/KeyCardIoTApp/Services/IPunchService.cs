using KeyCardIoTApp.Models;
using Refit;

namespace KeyCardIoTApp.Services;

public interface IPunchService
{
    [Post("/api/{version}/punch")]
    Task<PunchViewModel> RegisterPunch([Header("x-physicalauth")] string authToken, string version = "v1");
}
