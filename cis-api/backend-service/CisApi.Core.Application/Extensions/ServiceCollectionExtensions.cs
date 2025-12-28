using CisApi.Core.Application.Services;
using CisApi.Core.Domain.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // TODO: Once implemented add AddScoped implementation of ITopicCommandService, ITopicQueryService, and so on for example:         services.AddScoped<ITopicCommandService, TopicCommandService>();

            services.AddScoped<ITopicCommandService, TopicCommandService>();
            services.AddScoped<IVoteCommandService, VoteCommandService>();
            services.AddScoped<ITopicQueryService, TopicQueryService>();
            services.AddScoped<IIdeaCommandService, IdeaCommandService>();
            services.AddScoped<IIdeaQueryService, IdeaQueryService>();

            services.AddScoped<IUserQueryService, UserQueryService>();

            return services;
        }
    }
}
