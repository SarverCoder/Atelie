using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using Atelie.Application.Options;

namespace Atelie.Api.Extensions;

public static class Extensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions(configuration);

        services.AddAuth(configuration);

        services.AddSwaggerConfigurations();

        services.AddCrossOriginResource();

        services.AddSignalR();

        services.AddMemoryCache();

        services.AddLogging();

        services.AddBackgroundJobs();

        services.AddDirectoryBrowser();
        services.AddHttpContextAccessor();
        services.AddDirectoryBrowser();

        services.AddMonitoring(configuration);

        return services;
    }

    public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOption>(configuration.GetSection("JwtOption"));
    }

    public static void AddAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("JwtOption").Get<JwtOption>();

        if (jwtOptions == null)
        {
            throw new InvalidOperationException("JWT sozlamalari topilmadi. appsettings.json faylida 'JwtOption' bo'limini tekshiring.");
        }

        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
            });
    }


    public static void AddSwaggerConfigurations(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Atelie WebApi", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
    }

    public static void AddCrossOriginResource(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder
                    // .WithOrigins("http://localhost:3000", "https://yourdomain.com") // Frontend URL
                    //.WithOrigins("http://127.0.0.1:5500")
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
        });
    }

    public static void AddBackgroundJobs(this IServiceCollection services)
    {
        //services.AddHostedService<StoreTariffManagementService>();
        //services.AddHostedService<OrderRemainingService>();
    }
    public static void AddMonitoring(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((serviceProvider, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom
                .Configuration(configuration);
        });
    }
}