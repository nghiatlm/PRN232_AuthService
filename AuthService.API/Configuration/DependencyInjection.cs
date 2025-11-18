using AuthService.Repository.Configuration;
using AuthService.Service.Configuration;

namespace AuthService.API.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddServiceDI().AddRepoDI().AddSwaggerDependencies();
            return services;
        }
    }
}