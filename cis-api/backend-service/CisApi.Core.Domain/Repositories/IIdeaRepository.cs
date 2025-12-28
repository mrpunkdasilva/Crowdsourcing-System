using CisApi.Core.Domain.Entities;

namespace CisApi.Core.Domain.Repositories;

public interface IIdeaRepository
{
    /// <summary>
    /// Creates a new idea in the database.
    /// </summary>
    Task<Idea> CreateIdeaAsync(User user, Idea idea);
    Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(int topicId);
    Task<Idea?> GetByIdAsync(int ideaId);

}