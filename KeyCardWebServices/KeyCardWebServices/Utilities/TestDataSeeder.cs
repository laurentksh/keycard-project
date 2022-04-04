using KeyCardWebServices.Data;
using KeyCardWebServices.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace KeyCardWebServices.Utilities;

public static class TestDataSeeder
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        await userManager.CreateAsync(new AppUser
        {
            UserName = "Admin",
            Email = "admin@localhost",
        }, "Password123$");
    }
}
