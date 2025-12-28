using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CisApi.Infrastructure.MongoDb;
using CisApi.Infrastructure.MongoDb.Repositories;
using CisApi.Core.Domain.Repositories;
using MongoDB.Driver;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            //  services.AddDbContext<AppDbContext>(options => options.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 36))));

            var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            services.AddSingleton<IMongoClient>(client);

            services.AddScoped<MongoDbContext>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IIdeaRepository, IdeaRepository>();
            services.AddScoped<IVoteRepository, VoteRepository>();

            services.AddAutoMapper(typeof(CisApi.Infrastructure.MongoDb.MappingProfiles.TopicProfile));
            services.AddAutoMapper(typeof(CisApi.Infrastructure.MongoDb.MappingProfiles.IdeaProfile));

            return services;
        }
    }
}