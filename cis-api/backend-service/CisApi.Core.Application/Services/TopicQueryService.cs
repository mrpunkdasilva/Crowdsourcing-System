using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using CisApi.Core.Domain.Services;

namespace CisApi.Core.Application.Services
{
    public class TopicQueryService : ITopicQueryService
    {
        private readonly ITopicRepository _repository;

        public TopicQueryService(ITopicRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<Topic>> GetTopicsAsync()
        {
              var topics = await _repository.GetTopicsAsync();
            return topics ?? Enumerable.Empty<Topic>();
        }

        public async Task<Topic?> GetTopicAsync(int id)
        {
             var topics = await _repository.GetTopicAsync(id);
            Console.WriteLine($"DEBUG: Topics found = {topics != null}");
            return topics;
        }
    }
}

