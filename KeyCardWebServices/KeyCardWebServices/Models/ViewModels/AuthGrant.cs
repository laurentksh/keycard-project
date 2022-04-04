using KeyCardWebServices.Data.Models;

namespace KeyCardWebServices.Models.ViewModels;

public class AuthGrantViewModel
{
    public Guid Id { get; set; }

    public string Token { get; set; }

    public string Device { get; set; }

    public AuthGrantType Type { get; set; }

    public Guid IssuedToId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public static AuthGrantViewModel FromAuthGrant(AuthGrant authGrant) => new()
    {
        Id = authGrant.Id,
        Token = authGrant.Token,
        Device = authGrant.Device,
        Type = authGrant.Type,
        IssuedToId = authGrant.IssuedToId,
        CreationDate = authGrant.CreationDate,
        ExpirationDate = authGrant.ExpirationDate
    };
}
