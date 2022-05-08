namespace KeyCardWebServices.Data.Models;

public class AuthGrant
{
    public Guid Id { get; set; }

    public string Token { get; set; }

    public string Device { get; set; }

    public AuthGrantType Type { get; set; }

    public Guid IssuedToId { get; set; }

    public AppUser IssuedTo { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpirationDate { get; set; }
}

public enum AuthGrantType
{
    Unknown = 0,
    Jwt = 1,
    DeviceKey = 2
}