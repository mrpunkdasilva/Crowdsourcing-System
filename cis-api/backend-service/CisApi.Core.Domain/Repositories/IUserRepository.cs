
using CisApi.Core.Domain.Entities;
using System.Threading.Tasks;

namespace CisApi.Core.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(string username, string password);
    }
}
