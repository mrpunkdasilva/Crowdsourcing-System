using CisApi.Infrastructure.UsersApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCisServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServices();

            //  services.AddInfrastructureServices(configuration);

            var dbType = configuration["DatabaseProvider"] ?? "mysql";

            if (dbType.Equals("mongodb", StringComparison.OrdinalIgnoreCase))
            {
                services.AddMongoDbServices(configuration);
            }
            else
            {
                services.AddMySQLServices(configuration);
            }

            services.AddPresentationServices();
            services.AddUserApiServices(configuration);

            return services;
        }
    }
}
