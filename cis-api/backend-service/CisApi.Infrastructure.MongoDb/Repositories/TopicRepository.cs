using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using MongoDB.Driver;
using CisApi.Infrastructure.MongoDb.Entities;
using Microsoft.Extensions.Logging;

namespace CisApi.Infrastructure.MongoDb.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TopicRepository> _logger;

        public TopicRepository(MongoDbContext context, IMapper mapper, ILogger<TopicRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Topic> CreateTopicAsync(User user, Topic topic)
        {

            Topic? createdTopic = null;
             
            try
            {
                 
                var userEntity = await _context.Users
                    .Find(u => u.login == user.Login)
                    .FirstOrDefaultAsync();


                if (userEntity == null)
                {
                    throw new InvalidOperationException($"User with login '{user.Login}' not found.");
                }

                var topicEntity = _mapper.Map<TopicEntity>(topic);

                topicEntity.id = (int)await _context.Topics.CountDocumentsAsync(FilterDefinition<TopicEntity>.Empty) + 1;

                topicEntity.CreatedBy = userEntity;
                
                topicEntity.CreatedAt = DateTime.UtcNow;

                await _context.Topics.InsertOneAsync(topicEntity);

                try
                {
                    createdTopic = _mapper.Map<Topic>(topicEntity);
                }
                catch (Exception mapEx)
                {
                    _logger.LogError(mapEx, "Mapping TopicEntity -> Topic failed.");
                }
                
                return createdTopic;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in CreateTopicAsync"); 
                return createdTopic;
            }
        }

        /// <summary>
        /// Asynchronously retrieves a list of all topics from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of domain <see cref="Topic"/> objects.</returns>
        public async Task<IEnumerable<Topic>> GetTopicsAsync()
        {
           try
            {
                var topicEntities = await _context.Topics.Find(_ => true).ToListAsync();

                if (topicEntities == null || topicEntities.Count == 0)
                    return new List<Topic>();

                var topics = _mapper.Map<List<Topic>>(topicEntities);

                return topics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetTopicsAsync"); 
                return null;
            }
        }

        /// <summary>
        /// Asynchronously retrieves a topic by its ID. This method is not yet implemented.
        /// </summary>
        /// <param name="id">The ID of the topic to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the topic.</returns>
        public Task<Topic> GetTopicAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
