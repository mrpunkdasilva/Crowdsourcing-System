
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CisApi.Infrastructure.UsersApi.Clients
{
    public class UsersApiClient : IUsersApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UsersApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<UserInfo?> GetUserAsync(string username, string password)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var baseAddress = _configuration.GetValue<string>("UsersApi:BaseAddress");
            if (string.IsNullOrEmpty(baseAddress))
            {
                throw new InvalidOperationException("UsersApi:BaseAddress is not configured.");
            }
            httpClient.BaseAddress = new Uri(baseAddress);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/auth");

            var authString = $"{username}:{password}";
            var encodedAuth = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authString));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", encodedAuth);

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserInfo>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
        }
    }
}
