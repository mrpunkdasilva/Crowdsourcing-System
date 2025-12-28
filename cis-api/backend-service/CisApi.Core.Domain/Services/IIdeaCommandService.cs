using System.Threading.Tasks;
using CisApi.Core.Domain.Entities;

namespace CisApi.Core.Domain.Services;

public interface IIdeaCommandService
{
    /// <summary>
    /// Creates a new idea for a given topic.
    /// </summary>
    /// <param name="user">The user creating the idea.</param>
    /// <param name="idea">The idea to create.</param>
    /// <returns>The created idea.</returns>
    Task<Idea> CreateIdeaAsync(User user, Idea idea);
    
    /// <summary>
    /// Retrieves an idea by its ID for a specific topic.
    /// </summary>
    /// <param name="topicId">The ID of the topic.</param>
    /// <param name="ideaId">The ID of the idea.</param>
    /// <returns>The found idea.</returns>
    Task<Idea> GetIdeaByIdAsync(int topicId, int ideaId);
    
    /// <summary>
    /// Creates a new idea for a given topic.
    /// </summary>
    /// <param name="topicId">The ID of the topic.</param>
    /// <param name="title">The title of the new idea.</param>
    /// <param name="description">The description of the new idea.</param>
    /// <param name="user">The user creating the idea.</param>
    /// <returns>The created idea.</returns>
    Task<Idea> CreateIdeaForTopicAsync(int topicId, string title, string description, User user);
}