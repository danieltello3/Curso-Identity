using Galaxy.Security.Application.InPorts.Reclamo;
using Galaxy.Security.Application.InPorts.Users;
using Galaxy.Security.Application.UseCases.Reclamo;
using Galaxy.Security.Application.UseCases.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Galaxy.Security.Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<ILoginUseCase, LoginUseCase>();
            services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
            services.AddScoped<IRemoveCookiesUseCase, RemoveCookiesUseCase>();
            services.AddScoped<ICreateReclamoUseCase, CreateReclamoUseCase>();
            services.AddScoped<IGetReclamoByCodeUseCase, GetReclamoByCodeUseCase>();
            return services;
        }
    }
}
