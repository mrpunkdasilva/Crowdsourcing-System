using CisApi.Core.Domain.Entities;

namespace CisApi.Core.Domain.Services;

public interface ITopicCommandService
{
    Task<Topic> CreateTopicAsync(User user, Topic topic);
}
