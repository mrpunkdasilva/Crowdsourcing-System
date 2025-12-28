
using CisApi.Core.Domain.Entities;
using System.Threading.Tasks;

namespace CisApi.Core.Domain.Services
{
    public interface IUserQueryService
    {
        Task<User?> GetUserAsync(string username, string password);
    }
}
