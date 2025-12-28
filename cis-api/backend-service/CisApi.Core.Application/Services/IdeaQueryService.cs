using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using CisApi.Core.Domain.Services;

namespace CisApi.Core.Application.Services;

    /// <summary>
    /// Application service for idea query operations.
    /// </summary>
    public class IdeaQueryService : IIdeaQueryService
    {
        private readonly IIdeaRepository _repository;

        public IdeaQueryService(IIdeaRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(int topicId)
        {
            return await _repository.GetIdeasByTopicIdAsync(topicId);
        
    }
}