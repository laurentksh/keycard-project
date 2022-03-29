using Microsoft.AspNetCore.Identity;

namespace KeyCardWebServices.Data.Models;

public class AppUser : IdentityUser<Guid>
{
    public List<AuthGrant> AuthGrants { get; set; }
}

public class AppRole : IdentityRole<Guid>
{

}