using KeyCardWebServices.Data.Models;
using KeyCardWebServices.Models.Dtos;
using KeyCardWebServices.Models.ViewModels;

namespace KeyCardWebServices.Services;

public interface IPunchService
{
    Task<PunchViewModel> RegisterPunch(AppUser user);

    Task<PunchViewModel> EditPunch(AppUser user, PunchEditDto editDto);

    Task<PunchViewModel> DeletePunch(AppUser user, Guid punchId);

    Task<PunchViewModel> GetPunch(AppUser user, Guid punchId);

    Task<List<PunchViewModel>> GetPunches(AppUser user, PunchFilterDto filter);
}
