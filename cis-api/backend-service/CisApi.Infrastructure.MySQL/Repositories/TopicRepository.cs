using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MySqlEntities = CisApi.Infrastructure.MySQL.Entities;

namespace CisApi.Infrastructure.MySQL.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TopicRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Topic> CreateTopicAsync(User user, Topic topic)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (topic is null)
            {
                throw new ArgumentNullException(nameof(topic));
            }
            
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

            if (userEntity == null)
            {
                // This should not happen if authentication is working correctly
                // but as a safeguard, we can create a new user.
                // However, the user story implies the user should exist.
                // For now, we will let it throw an exception if the user is not found,
                // which will result in a 500 error.
                // A better approach would be to handle this case explicitly.
                // Based on the user story, the user should be created before this call.
                throw new InvalidOperationException($"User with login '{user.Login}' not found.");
            }

            var topicEntity = _mapper.Map<MySqlEntities.TopicEntity>(topic);
            topicEntity.CreatedBy = userEntity;
            topicEntity.CreatedAt = DateTime.UtcNow;

            await _context.Topics.AddAsync(topicEntity);
            await _context.SaveChangesAsync();

            // After saving, the topicEntity will have the ID and the CreatedBy navigation property populated.
            // We need to map it back to the domain entity to return it.
            var createdTopic = _mapper.Map<Topic>(topicEntity);

            return createdTopic;
        }

        /// <summary>
        /// Asynchronously retrieves a list of all topics from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of domain <see cref="Topic"/> objects.</returns>
        public async Task<IEnumerable<Topic>> GetTopicsAsync()
        {
            var topicEntities = await _context.Topics
                .Include(t => t.CreatedBy)
                .Include(t => t.Ideas)
                .ToListAsync();
            return _mapper.Map<IEnumerable<Topic>>(topicEntities);
        }

        /// <summary>
        /// Asynchronously retrieves a topic by its ID. This method is not yet implemented.
        /// </summary>
        /// <param name="id">The ID of the topic to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the topic.</returns>
        public Task<Topic?> GetTopicAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
