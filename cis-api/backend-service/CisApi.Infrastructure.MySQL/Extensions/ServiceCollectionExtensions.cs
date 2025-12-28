using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CisApi.Infrastructure.MySQL;
using CisApi.Infrastructure.MySQL.Repositories;
using CisApi.Core.Domain.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMySQLServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("MySQL"),
                    new MySqlServerVersion(new Version(8, 0, 36))));

            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IVoteRepository, VoteRepository>();
            services.AddScoped<IIdeaRepository, IdeaRepository>();

            services.AddAutoMapper(typeof(CisApi.Infrastructure.MySQL.MappingProfiles.TopicProfile));
            services.AddAutoMapper(typeof(CisApi.Infrastructure.MySQL.MappingProfiles.IdeaProfile));

            return services;
        }
    }
}