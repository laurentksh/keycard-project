namespace KeyCardWebServices.Data.Models;

public class Punch
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public AppUser User { get; set; }

    public Guid UserId { get; set; }

    public PunchSource Source { get; set; }

    public static PunchSource FromAuthGrantType(AuthGrantType type) => type switch
    {
        AuthGrantType.Unknown => PunchSource.Unknown,
        AuthGrantType.Jwt => PunchSource.WebPortal,
        AuthGrantType.Physical => PunchSource.Physical,
        _ => throw new NotImplementedException(),
    };
}

public enum PunchSource
{
    Unknown = 0,
    WebPortal = 1,
    Physical = 2
}