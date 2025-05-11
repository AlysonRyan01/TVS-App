using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TVS_App.Application.Handlers;
using TVS_App.Application.Interfaces;
using TVS_App.Application.Repositories;
using TVS_App.Infrastructure.Data;
using TVS_App.Infrastructure.Models;
using TVS_App.Infrastructure.Repositories;
using TVS_App.Infrastructure.Security;
using TVS_App.Infrastructure.Services;

namespace TVS_App.Api.Common;

public static class BuilderExtensions
{
    public static void AddSqlServer(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
    }

    public static void AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer" ?? ""],
                    ValidAudience = builder.Configuration["Jwt:Audience" ?? ""],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
                };
            });

        builder.Services.AddAuthorization();
    }

    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
        builder.Services.AddTransient<IGenerateServiceOrderPdf, GenerateServiceOrderPdfService>();
        builder.Services.AddScoped<CustomerHandler>();
        builder.Services.AddScoped<ServiceOrderHandler>();
    }

    public static void AddJwtService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<JwtService>();
    }

    public static void AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDataContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureJsonSerializer(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        });
    }
}