using CisApi.Core.Domain.Entities;

namespace CisApi.Core.Domain.Repositories
{
    public interface IVoteRepository
    {
        Task<Idea> CreateVoteAsync(User user, int ideaId);

        Task<Idea> DeleteVoteAsync(User user, int ideaId);
    }
}
