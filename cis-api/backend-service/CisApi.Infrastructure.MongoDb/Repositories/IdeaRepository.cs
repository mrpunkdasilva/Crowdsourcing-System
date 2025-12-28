using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using MongoDB.Driver;
using CisApi.Infrastructure.MongoDb.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace CisApi.Infrastructure.MongoDb.Repositories
{
    /// <summary>
    /// MongoDB implementation of IIdeaRepository for READ and WRITE operations.
    /// </summary>
    public class IdeaRepository : IIdeaRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<IdeaRepository> _logger;

        public IdeaRepository(MongoDbContext context, IMapper mapper, ILogger<IdeaRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates a new idea in MongoDB.
        /// </summary>
        public async Task<Idea> CreateIdeaAsync(User user, Idea idea)
        {
            Idea? createdIdea = null;

            try
            {
                var userEntity = await _context.Users
                    .Find(u => u.login == user.Login)
                    .FirstOrDefaultAsync();

                if (userEntity == null)
                {
                    throw new InvalidOperationException($"User with login '{user.Login}' not found.");
                }

                var ideaEntity = _mapper.Map<IdeaEntity>(idea);

                ideaEntity.Id = (int)await _context.Ideas.CountDocumentsAsync(FilterDefinition<IdeaEntity>.Empty) + 1;

                ideaEntity.CreatedBy = userEntity;
                ideaEntity.CreatedAt = DateTime.UtcNow;
                ideaEntity.VoteCount = 0;
                ideaEntity.VotedBy = new List<IdeaVotesEntity>();

                await _context.Ideas.InsertOneAsync(ideaEntity);

                try
                {
                    createdIdea = _mapper.Map<Idea>(ideaEntity);
                }
                catch (Exception mapEx)
                {
                    _logger.LogError(mapEx, "Mapping IdeaEntity -> Idea failed.");
                }

                _logger.LogInformation($"[MongoDB] Idea created: {ideaEntity.Title} (ID: {ideaEntity.Id})");
                return createdIdea;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in CreateIdeaAsync");
                return createdIdea;
            }
        }

        /// <summary>
        /// Retrieves ideas by topic ID.
        /// </summary>
        public async Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(int topicId)
        {
            List<Idea> ideas = null;

            try
            {
                var ideaEntities = await _context.Ideas
                    .Find(i => i.TopicId == topicId)
                    .SortByDescending(i => i.CreatedAt)
                    .ToListAsync();

                if (ideaEntities == null || !ideaEntities.Any())
                {
                    _logger.LogInformation($"[MongoDB] No ideas found for topic {topicId}");
                    return new List<Idea>();
                }

                ideas = new List<Idea>();

                foreach (var ideaEntity in ideaEntities)
                {
                    try
                    {
                        var idea = _mapper.Map<Idea>(ideaEntity);

                        if (ideaEntity.CreatedBy != null)
                        {
                            idea.CreatedBy = new User
                            {
                                Id = ideaEntity.CreatedBy.MongoId,
                                Login = ideaEntity.CreatedBy.login
                            };
                        }

                        if (ideaEntity.VotedBy != null && ideaEntity.VotedBy.Any())
                        {
                            idea.VotedBy = ideaEntity.VotedBy
                                .Select(v => v.User.MongoId)
                                .ToList();
                        }
                        else
                        {
                            idea.VotedBy = new List<string>();
                        }

                        ideas.Add(idea);
                    }
                    catch (Exception mapEx)
                    {
                        _logger.LogError(mapEx, $"Mapping idea {ideaEntity.Id} failed.");
                    }
                }

                _logger.LogInformation($"[MongoDB] Found {ideas.Count} ideas for topic {topicId}");
                return ideas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetIdeasByTopicIdAsync");
                return ideas ?? new List<Idea>();
            }
        }

        /// <summary>
        /// Retrieves an idea by its ID.
        /// </summary>
        public async Task<Idea?> GetByIdAsync(int id)
        {
            Idea? foundIdea = null;

            try
            {
                var ideaEntity = await _context.Ideas
                    .Find(i => i.Id == id)
                    .FirstOrDefaultAsync();

                if (ideaEntity == null)
                {
                    _logger.LogInformation($"[MongoDB] Idea with ID {id} not found");
                    return null;
                }

                try
                {
                    foundIdea = _mapper.Map<Idea>(ideaEntity);

                    if (ideaEntity.CreatedBy != null)
                    {
                        foundIdea.CreatedBy = new User
                        {
                            Id = ideaEntity.CreatedBy.MongoId,
                            Login = ideaEntity.CreatedBy.login
                        };
                    }

                    if (ideaEntity.VotedBy != null && ideaEntity.VotedBy.Any())
                    {
                        foundIdea.VotedBy = ideaEntity.VotedBy
                            .Select(v => v.User.MongoId)
                            .ToList();
                    }
                    else
                    {
                        foundIdea.VotedBy = new List<string>();
                    }
                }
                catch (Exception mapEx)
                {
                    _logger.LogError(mapEx, "Mapping IdeaEntity -> Idea failed.");
                }

                _logger.LogInformation($"[MongoDB] Found idea: {ideaEntity.Title} (ID: {id})");
                return foundIdea;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetByIdAsync");
                return foundIdea;
            }
        }
    }
}
