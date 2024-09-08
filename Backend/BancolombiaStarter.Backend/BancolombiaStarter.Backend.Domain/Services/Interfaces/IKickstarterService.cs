using Newtonsoft.Json.Linq;

namespace BancolombiaStarter.Backend.Domain.Services.Interfaces
{
    public interface IKickstarterService
    {
        Task<JObject> GetProjectByIdAsync(string projectId);
        Task<JArray> SearchProjectsAsync(string query);

    }
}
