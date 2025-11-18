
using AuthService.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Service.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceDI(this IServiceCollection services)
        {
            var userServiceUrl = Environment.GetEnvironmentVariable("USER_SERVICE_URL") ?? "https://localhost:7220";

            services.AddHttpClient<IAccountService, AccountService>(client =>
            {
                client.BaseAddress = new Uri(userServiceUrl);
            });
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}