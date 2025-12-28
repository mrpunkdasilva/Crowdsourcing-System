
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using CisApi.Infrastructure.UsersApi.Clients;
using System.Threading.Tasks;

namespace CisApi.Infrastructure.UsersApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUsersApiClient _usersApiClient;

        public UserRepository(IUsersApiClient usersApiClient)
        {
            _usersApiClient = usersApiClient;
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            var userInfo = await _usersApiClient.GetUserAsync(username, password);
            if (userInfo == null)
            {
                return null;
            }

            return new User
            {
                Id = userInfo.Id,
                Login = userInfo.Login
            };
        }
    }
}
