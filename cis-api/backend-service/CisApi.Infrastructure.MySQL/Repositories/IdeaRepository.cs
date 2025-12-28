using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using CisApi.Infrastructure.MySQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CisApi.Infrastructure.MySQL.Repositories;
    public class IdeaRepository : IIdeaRepository
    {
        private readonly AppDbContext _context;

        public IdeaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Idea> CreateIdeaAsync(User user, Idea idea)
        {
            var topic = await _context.Topics.FindAsync(idea.TopicId);
           
            if (topic == null)
            {
                throw new InvalidOperationException($"Topic with ID {idea.TopicId} not found.");
            }

            var userEntity = await _context.Users.FindAsync(user.Login);
            
            if (userEntity == null)
            {
                throw new InvalidOperationException($"User with login '{user.Login}' not found for creating idea.");
            }
            
            var entity = new IdeaEntity
            {
                Title = idea.Title,
                Description = idea.Description,
                TopicId = idea.TopicId,
                CreatedById = user.Id ?? "1",
                VoteCount = 0,
                CreatedAt = DateTime.UtcNow,
                Topic = topic,
                CreatedBy = userEntity
            };

            _context.Ideas.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity).Reference(i => i.CreatedBy).LoadAsync();
            await _context.Entry(entity).Collection(i => i.Votes).LoadAsync();

            var votedByList = new List<string>();
            if (entity.Votes != null && entity.Votes.Any())
            {
                votedByList = entity.Votes
                    .Where(v => v.UserId != null)
                    .Select(v => v.UserId!)
                    .ToList();
            }

            return new Idea
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                TopicId = entity.TopicId,
                VotedBy = votedByList,
                VoteCount = (int)entity.VoteCount,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy == null
                    ? new User { Id = entity.CreatedById, Login = string.Empty }
                    : new User
                    {
                        Id = entity.CreatedBy.Id,
                        Login = entity.CreatedBy.Login
                    }
            };
        }

        public async Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(int topicId)
        {
            var entities = await _context.Ideas
                .Include(i => i.CreatedBy)
                .Include(i => i.Votes)
                .Where(i => i.TopicId == topicId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return entities.Select(entity =>
            {
                var votedByList = new List<string>();
                if (entity.Votes != null && entity.Votes.Any())
                {
                    votedByList = entity.Votes
                        .Where(v => v.UserId != null)
                        .Select(v => v.UserId!)
                        .ToList();
                }

                return new Idea
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Description = entity.Description,
                    TopicId = entity.TopicId,
                    VotedBy = votedByList,
                    VoteCount = (int)entity.VoteCount,
                    CreatedAt = entity.CreatedAt,
                    CreatedBy = entity.CreatedBy == null
                        ? new User { Id = entity.CreatedById, Login = string.Empty }
                        : new User
                        {
                            Id = entity.CreatedBy.Id,
                            Login = entity.CreatedBy.Login
                        }
                };
            });
        }
        public async Task<Idea?> GetByIdAsync(int id)
        {
            var entity = await _context.Ideas
                .Include(i => i.CreatedBy)
                .Include(i => i.Votes)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (entity == null)
                return null;

            var votedByList = new List<string>();
            if (entity.Votes != null && entity.Votes.Any())
            {
                votedByList = entity.Votes
                    .Where(v => v.UserId != null)
                    .Select(v => v.UserId!)
                    .ToList();
            }

            return new Idea
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                TopicId = entity.TopicId,
                VotedBy = votedByList,
                VoteCount = (int)entity.VoteCount,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy == null
                    ? new User { Id = entity.CreatedById, Login = string.Empty }
                    : new User
                    {
                        Id = entity.CreatedBy.Id,
                        Login = entity.CreatedBy.Login
                    }
            };
        }

}