using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using CisApi.Core.Domain.Services;

namespace CisApi.Core.Application.Services
{
    public class VoteCommandService : IVoteCommandService
    {
        private readonly IVoteRepository _voteRepository;

        public VoteCommandService(IVoteRepository voteRepository)
        {
            _voteRepository = voteRepository;
        }

        public async Task<Idea> CreateVoteAsync(User user, int ideaId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return await _voteRepository.CreateVoteAsync(user, ideaId);
        }

        public async Task<Idea> DeleteVoteAsync(User user, int ideaId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return await _voteRepository.DeleteVoteAsync(user, ideaId);
        }
    }
}
