
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Repository.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepoDI(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}