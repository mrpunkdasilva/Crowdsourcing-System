
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using CisApi.Core.Domain.Services;
using System.Threading.Tasks;

namespace CisApi.Core.Application.Services
{
    public class UserQueryService : IUserQueryService
    {
        private readonly IUserRepository _userRepository;

        public UserQueryService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            return await _userRepository.GetUserAsync(username, password);
        }
    }
}
