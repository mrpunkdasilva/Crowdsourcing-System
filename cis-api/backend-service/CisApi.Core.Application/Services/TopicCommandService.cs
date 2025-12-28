using System;
using System.Threading.Tasks;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using CisApi.Core.Domain.Services;


namespace CisApi.Core.Application.Services
{
    public class TopicCommandService : ITopicCommandService
    {
        private readonly ITopicRepository _repository;

        public TopicCommandService(ITopicRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Topic> CreateTopicAsync(User user, Topic topic)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (topic == null)
                throw new ArgumentNullException(nameof(topic));

            return await _repository.CreateTopicAsync(user, topic);
        }
    } 
}



