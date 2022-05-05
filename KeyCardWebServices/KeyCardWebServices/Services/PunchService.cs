using KeyCardWebServices.Data;
using KeyCardWebServices.Data.Models;
using KeyCardWebServices.Exceptions;
using KeyCardWebServices.Models.Dtos;
using KeyCardWebServices.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KeyCardWebServices.Services;

public class PunchService : IPunchService
{
    private readonly ApplicationDbContext _context;

    public PunchService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PunchViewModel> RegisterPunch(AppUser user, PunchSource source)
    {
        var entry = await _context.Punches.AddAsync(new Punch
        {
            CreationDate = DateTime.UtcNow,
            Source = source,
            UserId = user.Id
        });
        await _context.SaveChangesAsync();

        return PunchViewModel.FromPunch(entry.Entity);
    }

    public async Task<PunchViewModel> EditPunch(AppUser user, Guid id, PunchEditDto editDto)
    {
        var punch = await _context.Punches.SingleOrDefaultAsync(x => x.Id == id);

        if (punch == null)
            throw new HttpException(System.Net.HttpStatusCode.NotFound);

        punch.CreationDate = editDto.NewDate;

        await _context.SaveChangesAsync();

        return PunchViewModel.FromPunch(punch);
    }

    public async Task<PunchViewModel> DeletePunch(AppUser user, Guid punchId)
    {
        var punch = await _context.Punches.SingleOrDefaultAsync(x => x.Id == punchId);

        if (punch == null)
            throw new HttpException(System.Net.HttpStatusCode.NotFound);

        _context.Punches.Remove(punch);
        await _context.SaveChangesAsync();

        return PunchViewModel.FromPunch(punch);
    }

    public async Task<PunchViewModel> GetPunch(AppUser user, Guid punchId)
    {
        var punch = await _context.Punches.SingleOrDefaultAsync(x => x.Id == punchId);

        if (punch == null)
            throw new HttpException(System.Net.HttpStatusCode.NotFound);

        if (punch.UserId != user.Id)
            throw new HttpException(System.Net.HttpStatusCode.Unauthorized);

        return PunchViewModel.FromPunch(punch);
    }

    public async Task<List<PunchViewModel>> GetPunches(AppUser user, PunchFilterDto? filter)
    {
        var punches = await _context.Punches.Where(x => x.UserId == user.Id).ToListAsync();

        if (filter != null && filter.Date != null)
            punches = punches.Where(x => x.CreationDate.Date == filter.Date.Value.Date).ToList();

        return punches.Select(x => PunchViewModel.FromPunch(x)).ToList();
    }
}
