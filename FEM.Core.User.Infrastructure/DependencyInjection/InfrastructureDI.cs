using System.Text;
using FEM.Core.User.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using DomainUser = FEM.Core.User.Domain.Entities;

namespace FEM.Core.User.Infrastructure.DependencyInjection;

public static class InfrastructureDI
{
    /// <summary>
    /// Adding the Database service and Jwt token service to the application.
    /// </summary>
    /// <param name="services">IserviceCollection for adding services</param>
    /// <param name="config">To access the configuration files</param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(
                config.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(InfrastructureDI).Assembly.FullName)), ServiceLifetime.Scoped
            );

        services.AddIdentity<DomainUser.User, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 4;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
                };
            });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("SuperAdmin"));
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("User", policy => policy.RequireRole("User"));
        });


        return services;

    }
}
