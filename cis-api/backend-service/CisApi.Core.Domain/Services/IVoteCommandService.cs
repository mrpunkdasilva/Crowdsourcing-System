using CisApi.Core.Domain.Entities;

namespace CisApi.Core.Domain.Services
{
    public interface IVoteCommandService
    {
        Task<Idea> CreateVoteAsync(User user, int ideaId);

        Task<Idea> DeleteVoteAsync(User user, int ideaId);
    }
}
