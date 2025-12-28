using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using CisApi.Infrastructure.MongoDb.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace CisApi.Infrastructure.MongoDb.Repositories
{
    /// <summary>
    /// MongoDB implementation of IVoteRepository for vote operations.
    /// </summary>
    public class VoteRepository : IVoteRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<VoteRepository> _logger;
        private readonly IMongoCollection<IdeaEntity> _ideas;

        public VoteRepository(MongoDbContext context, IMapper mapper, ILogger<VoteRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ideas = _context.Ideas;
        }

        /// <summary>
        /// Creates a vote for an idea.
        /// </summary>
        public async Task<Idea> CreateVoteAsync(User user, int ideaId)
        {
            Idea? votedIdea = null;

            try
            {
                // 1. Buscar ideia no MongoDB
                var ideaEntity = await _ideas
                    .Find(i => i.Id == ideaId)
                    .FirstOrDefaultAsync();

                if (ideaEntity == null)
                {
                    _logger.LogWarning($"[MongoDB] Idea with ID {ideaId} not found");
                    return null;
                }

                // 2. Verificar se usuário já votou
                var alreadyVoted = ideaEntity.VotedBy?.Any(v => v.User.MongoId == user.Id) ?? false;
                if (alreadyVoted)
                {
                    _logger.LogWarning($"[MongoDB] User {user.Login} already voted on idea {ideaId}");
                    return null;
                }

                // 3. Buscar UserEntity completo do MongoDB
                var userEntity = await _context.Users
                    .Find(u => u.login == user.Login)
                    .FirstOrDefaultAsync();

                if (userEntity == null)
                {
                    throw new InvalidOperationException($"User with login '{user.Login}' not found.");
                }

                // 4. Criar voto embedded com UserEntity completo
                var voteEntity = new IdeaVotesEntity
                {
                    User = userEntity,
                    VotedAt = DateTime.UtcNow
                };

                // 5. Adicionar voto ao array e incrementar contador
                var filter = Builders<IdeaEntity>.Filter.Eq(i => i.Id, ideaId);
                var update = Builders<IdeaEntity>.Update
                    .Push(i => i.VotedBy, voteEntity)
                    .Inc(i => i.VoteCount, 1);

                await _ideas.UpdateOneAsync(filter, update);

                // 6. Buscar ideia atualizada
                var updatedIdeaEntity = await _ideas
                    .Find(i => i.Id == ideaId)
                    .FirstOrDefaultAsync();

                // 7. Mapear para domínio
                try
                {
                    votedIdea = _mapper.Map<Idea>(updatedIdeaEntity);

                    if (updatedIdeaEntity.CreatedBy != null)
                    {
                        votedIdea.CreatedBy = new User
                        {
                            Id = updatedIdeaEntity.CreatedBy.MongoId,
                            Login = updatedIdeaEntity.CreatedBy.login
                        };
                    }

                    if (updatedIdeaEntity.VotedBy != null && updatedIdeaEntity.VotedBy.Any())
                    {
                        votedIdea.VotedBy = updatedIdeaEntity.VotedBy
                            .Select(v => v.User.MongoId)
                            .ToList();
                    }
                }
                catch (Exception mapEx)
                {
                    _logger.LogError(mapEx, "Mapping IdeaEntity -> Idea failed.");
                }

                _logger.LogInformation($"[MongoDB] Vote created: User {user.Login} voted on idea {ideaId}");
                return votedIdea;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in CreateVoteAsync");
                return votedIdea;
            }
        }

        /// <summary>
        /// Removes a vote from an idea.
        /// </summary>
        public async Task<Idea> DeleteVoteAsync(User user, int ideaId)
        {
            Idea? unvotedIdea = null;

            try
            {
                // 1. Buscar ideia no MongoDB
                var ideaEntity = await _ideas
                    .Find(i => i.Id == ideaId)
                    .FirstOrDefaultAsync();

                if (ideaEntity == null)
                {
                    _logger.LogWarning($"[MongoDB] Idea with ID {ideaId} not found");
                    return null;
                }

                // 2. Verificar se usuário votou
                var hasVoted = ideaEntity.VotedBy?.Any(v => v.User.MongoId == user.Id) ?? false;
                if (!hasVoted)
                {
                    _logger.LogWarning($"[MongoDB] User {user.Login} has not voted on idea {ideaId}");
                    return null;
                }

                // 3. Remover voto do array e decrementar contador
                var filter = Builders<IdeaEntity>.Filter.Eq(i => i.Id, ideaId);
                var update = Builders<IdeaEntity>.Update
                    .PullFilter(i => i.VotedBy, v => v.User.MongoId == user.Id)
                    .Inc(i => i.VoteCount, -1);

                await _ideas.UpdateOneAsync(filter, update);

                // 4. Buscar ideia atualizada
                var updatedIdeaEntity = await _ideas
                    .Find(i => i.Id == ideaId)
                    .FirstOrDefaultAsync();

                // 5. Mapear para domínio
                try
                {
                    unvotedIdea = _mapper.Map<Idea>(updatedIdeaEntity);

                    if (updatedIdeaEntity.CreatedBy != null)
                    {
                        unvotedIdea.CreatedBy = new User
                        {
                            Id = updatedIdeaEntity.CreatedBy.MongoId,
                            Login = updatedIdeaEntity.CreatedBy.login
                        };
                    }

                    if (updatedIdeaEntity.VotedBy != null && updatedIdeaEntity.VotedBy.Any())
                    {
                        unvotedIdea.VotedBy = updatedIdeaEntity.VotedBy
                            .Select(v => v.User.MongoId)
                            .ToList();
                    }
                    else
                    {
                        unvotedIdea.VotedBy = new List<string>();
                    }
                }
                catch (Exception mapEx)
                {
                    _logger.LogError(mapEx, "Mapping IdeaEntity -> Idea failed.");
                }

                _logger.LogInformation($"[MongoDB] Vote removed: User {user.Login} unvoted from idea {ideaId}");
                return unvotedIdea;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteVoteAsync");
                return unvotedIdea;
            }
        }
    }
}
