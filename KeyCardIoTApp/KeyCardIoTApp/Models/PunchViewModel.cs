namespace KeyCardIoTApp.Models;

public class PunchViewModel
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public Guid UserId { get; set; }

    public PunchSource Source { get; set; }
}

public enum PunchSource
{
    Unknown = 0,
    WebPortal = 1,
    Physical = 2
}