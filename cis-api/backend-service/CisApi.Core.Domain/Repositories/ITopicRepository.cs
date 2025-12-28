using CisApi.Core.Domain.Entities;

namespace CisApi.Core.Domain.Repositories;

public interface ITopicRepository
{
    Task<Topic> CreateTopicAsync(User user, Topic topic);
    Task<IEnumerable<Topic>> GetTopicsAsync();
    Task<Topic?> GetTopicAsync(int id);

}
