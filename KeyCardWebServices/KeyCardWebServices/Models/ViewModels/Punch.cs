using KeyCardWebServices.Data.Models;

namespace KeyCardWebServices.Models.ViewModels;

public class PunchViewModel
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public Guid UserId { get; set; }

    public PunchSource Source { get; set; }

    public static PunchViewModel FromPunch(Punch punch) => new()
    {
        Id = punch.Id,
        CreationDate = punch.CreationDate,
        UserId = punch.UserId,
        Source = punch.Source
    };
}
