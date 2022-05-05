using System.Text;
using KeyCardWebServices.Auth;
using KeyCardWebServices.Data;
using KeyCardWebServices.Data.Models;
using KeyCardWebServices.Services;
using KeyCardWebServices.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace KeyCardWebServices;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddLogging(x =>
        {
            if (builder.Environment.IsProduction())
            {
                x.SetMinimumLevel(LogLevel.Information);
            } else
            {
                x.SetMinimumLevel(LogLevel.Debug);
                x.AddConsole();
            }
        });

        builder.Services.AddDbContext<ApplicationDbContext>(o =>
        {
            o.UseInMemoryDatabase("InMemoryDb");
        });

        builder.Services.AddIdentity<AppUser, AppRole>(x =>
        {
            x.Lockout.AllowedForNewUsers = false;
        })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddAuthentication("WebPortalAuth")
            .AddJwtBearer("WebPortalAuth", "Employee Login / JWT Token", x =>
            {
                var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtSigningKey"));

                x.SaveToken = true;

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateAudience = false,

                    RequireExpirationTime = true,
                    RequireAudience = false,
                    RequireSignedTokens = true,

                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                x.Validate("WebPortalAuth");
            })
            .AddScheme<PhysicalAuthenticationOptions, PhysicalAuthenticationHandler>("PhysicalAuth", "Physical NFC Auth", null!);

        builder.Services.AddAuthorization();

        builder.Services.AddControllers(x =>
        {
            x.Filters.Add<ExceptionFilter>();
        });

        builder.Services.AddApiVersioning(x =>
        {
            x.ReportApiVersions = true;
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            x.RegisterMiddleware = true;
        });

        builder.Services.AddVersionedApiExplorer(x =>
        {
            x.GroupNameFormat = "'v'VVV";
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddCors(x =>
        {
            x.AddDefaultPolicy(y => y
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
            );
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "KeyCard WebApi",
                Version = "v1"
            });
            
            //x.OperationFilter<CustomHeaderOperationFilter>("X-PHYSICALAUTH", "Physical NFC-based authentication");
            x.AddSecurityDefinition("WebPortalAuth", new OpenApiSecurityScheme
            {
                Description = "JWT employee authentication",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            x.AddSecurityDefinition("PhysicalAuth", new OpenApiSecurityScheme
            {
                Description = "Physical NFC-based authentication",
                In = ParameterLocation.Header,
                Name = "X-PHYSICALAUTH",
                Type = SecuritySchemeType.ApiKey
            });
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "WebPortalAuth"
                        }
                    },
                    Array.Empty<string>()
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "PhysicalAuth"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        builder.Services
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<IPunchService, PunchService>();

        var app = builder.Build();
        
        app.UseApiVersioning();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        using var scope = app.Services.CreateScope();
        await TestDataSeeder.Seed(scope.ServiceProvider);

        app.Run();
    }
}
