using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using CisApi.Infrastructure.MySQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CisApi.Infrastructure.MySQL.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public VoteRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Idea> CreateVoteAsync(User user, int ideaId)
        {
            var ideaEntity = await _context.Ideas
                .Include(i => i.Topic)
                .FirstOrDefaultAsync(i => i.Id == ideaId);

            if (ideaEntity == null)
                throw new Exception("Idea not found.");

            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == user.Login);

            if (userEntity == null)
                throw new Exception("User not found.");


            var ideaVote = await _context.IdeaVotes
                .FirstOrDefaultAsync(iv => iv.IdeaId == ideaEntity.Id && iv.UserId == userEntity.Id);

            if (ideaVote != null)
                throw new Exception("You already voted for this idea.");

            var ideaVotesEntity = new IdeaVotesEntity
            {
                IdeaId = ideaEntity.Id,
                UserId = userEntity.Id,
                Idea = ideaEntity,
                User = userEntity
            };

            _context.IdeaVotes.Add(ideaVotesEntity);
            await _context.SaveChangesAsync();

            ideaEntity.VoteCount += 1;

            await _context.SaveChangesAsync();

            return _mapper.Map<Idea>(ideaEntity);
        }
        
        public async Task<Idea> DeleteVoteAsync(User user, int ideaId)
        {
            var ideaEntity = await _context.Ideas
                .Include(i => i.Topic)
                .FirstOrDefaultAsync(i => i.Id == ideaId);

            if (ideaEntity == null)
                throw new Exception("Idea not found.");

            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == user.Login);

            if (userEntity == null)
                throw new Exception("User not found.");

            var ideaVote = await _context.IdeaVotes
                .FirstOrDefaultAsync(iv => iv.IdeaId == ideaEntity.Id && iv.UserId == userEntity.Id);

            if (ideaVote == null)
                throw new Exception("Vote not found.");

            _context.IdeaVotes.Remove(ideaVote);
            await _context.SaveChangesAsync();

            ideaEntity.VoteCount -= 1;
            await _context.SaveChangesAsync();

            return _mapper.Map<Idea>(ideaEntity);
        }
    }
}
