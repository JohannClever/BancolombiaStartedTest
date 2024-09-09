using Newtonsoft.Json;

namespace BancolombiaStarter.Backend.Domain.Dto
{
    public class SimilarProjectsResponse
    {
        [JsonProperty("SimilarProjectIds")]
        public List<long> SimilarProjectIds { get; set; }
    }
}
