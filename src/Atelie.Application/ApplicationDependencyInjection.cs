using Atelie.Application.Helpers.GenerateJwt;
using Atelie.Application.Helpers.PasswordHasher;
using Atelie.Application.Helpers.SmsProvider;
using Atelie.Application.Services;
using Atelie.Application.Services.Implementations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Atelie.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
    {
        services.AddServices(env);

        services.RegisterAutoMapper();

        services.AddHttpServices();

        return services;
    }

    private static void AddServices(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IFabricService, FabricService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<ISmsTokenProvider, SmsTokenProvider>();
        services.AddScoped<ISmsProvider, SmsProvider>();
    }
    private static void RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(IAuthService));
    }

    private static void AddHttpServices(this IServiceCollection services)
    {
        services.AddHttpClient<SmsTokenProvider>();
        services.AddHttpClient<SmsProvider>();
    }
}
