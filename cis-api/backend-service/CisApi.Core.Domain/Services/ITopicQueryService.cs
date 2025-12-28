using CisApi.Core.Domain.Entities;

namespace CisApi.Core.Domain.Services;

public interface ITopicQueryService
{
    Task<IEnumerable<Topic>> GetTopicsAsync();
    Task<Topic?> GetTopicAsync(int id);
}
