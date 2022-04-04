using KeyCardWebServices.Data.Models;
using KeyCardWebServices.Models.Dtos;
using KeyCardWebServices.Models.ViewModels;

namespace KeyCardWebServices.Services;

public class PunchService : IPunchService
{
    public Task<PunchViewModel> RegisterPunch(AppUser user)
    {
        throw new NotImplementedException();
    }

    public Task<PunchViewModel> EditPunch(AppUser user, PunchEditDto editDto)
    {
        throw new NotImplementedException();
    }

    public Task<PunchViewModel> DeletePunch(AppUser user, Guid punchId)
    {
        throw new NotImplementedException();
    }

    public Task<PunchViewModel> GetPunch(AppUser user, Guid punchId)
    {
        throw new NotImplementedException();
    }

    public Task<List<PunchViewModel>> GetPunches(AppUser user, PunchFilterDto filter)
    {
        throw new NotImplementedException();
    }
}
