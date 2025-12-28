using System.Collections.Generic;
using System.Threading.Tasks;
using CisApi.Core.Domain.Entities;

namespace CisApi.Core.Domain.Services;

    /// <summary>
    /// Service interface for idea query operations (Read).
    /// </summary>
    public interface IIdeaQueryService
    {
        /// <summary>
        /// Gets all ideas for a specific topic.
        /// </summary>
        /// <param name="topicId">The topic ID.</param>
        /// <returns>List of ideas. Empty list if no ideas found.</returns>
        Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(int topicId);
}