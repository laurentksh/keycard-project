using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeyCardWebServices.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{

}

public class AppUser : IdentityUser<Guid>
{

}

public class AppRole : IdentityRole<Guid>
{

}