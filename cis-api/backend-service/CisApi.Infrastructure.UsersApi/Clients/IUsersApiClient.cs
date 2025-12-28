
using System.Threading.Tasks;

namespace CisApi.Infrastructure.UsersApi.Clients
{
    public class UserInfo
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
    }

    public interface IUsersApiClient
    {
        Task<UserInfo?> GetUserAsync(string username, string password);
    }
}
