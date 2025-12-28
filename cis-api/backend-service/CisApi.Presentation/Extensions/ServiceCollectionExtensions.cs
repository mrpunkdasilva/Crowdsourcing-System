using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddAutoMapper(Assembly.Load("CisApi.Presentation"), Assembly.Load("CisApi.Infrastructure.MySQL"));

            return services;
        }
    }
}
