
using CisApi.Core.Domain.Repositories;
using CisApi.Infrastructure.UsersApi.Clients;
using CisApi.Infrastructure.UsersApi.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CisApi.Infrastructure.UsersApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddScoped<IUsersApiClient, UsersApiClient>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
