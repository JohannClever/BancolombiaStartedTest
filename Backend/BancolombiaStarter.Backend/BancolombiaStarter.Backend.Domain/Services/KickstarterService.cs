
using BancolombiaStarter.Backend.Domain.Services.Generic;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace BancolombiaStarter.Backend.Domain.Services
{
    public class KickstarterService: IKickstarterService
    {
        private readonly HttpClient _httpClient;

        public KickstarterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JObject> GetProjectByIdAsync(string projectId)
        {
            var response = await _httpClient.GetAsync($"https://api.kickstarter.com/v1/projects/{projectId}?key=your_api_key");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JObject.Parse(jsonResponse);
        }

        public async Task<JArray> SearchProjectsAsync(string query)
        {
            var response = await _httpClient.GetAsync($"https://api.kickstarter.com/v1/search?term={query}&key=your_api_key");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JArray.Parse(jsonResponse);
        }
    }

}
