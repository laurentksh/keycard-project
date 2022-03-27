using KeyCardWebServices.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace KeyCardWebServices;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<ApplicationDbContext>(o => {
            var host = builder.Environment;

            if (host.IsProduction())
                o.UseSqlite("Data Source=app.db");
            else
                o.UseInMemoryDatabase("InMemoryDb");
        });

        builder.Services.AddSignalR();

        builder.Services.AddAuthentication("WebPortalAuth")
            .AddJwtBearer("WebPortalAuth", "Employee Login / JWT Token", null!)
            .AddPolicyScheme("PhysicalAuth", "Physical NFC Auth");

        builder.Services.AddAuthorization();

        builder.Services.AddControllers();
        builder.Services.AddApiVersioning();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseApiVersioning();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
