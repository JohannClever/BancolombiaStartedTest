using BancolombiaStarter.Backend.Domain.Dto;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;
using OpenAI_API;
using OpenAI_API.Completions;
namespace BancolombiaStarter.Backend.Domain.Services
{
    public class OpenAiService: IOpenAiService
    {
        private readonly OpenAIAPI _api;

        public OpenAiService(string apiKey)
        {
            _api = new OpenAIAPI(apiKey);
        }

        public async Task<OpenAiResponse> GetCampaignSuggestionsAsync(string campaignDescription)
        {
            var result = await _api.Completions.CreateCompletionAsync(new CompletionRequest
            {
                Prompt = $"Provide suggestions for improving the following campaign description:\n\n{campaignDescription}\n\nSuggestions:",
                MaxTokens = 150,
                Temperature = 0.7,
            });

            var firstCompletion = result.Completions.FirstOrDefault();

            return new OpenAiResponse
            {
                Id = result.Id,
                Object = result.Object,
                Created = result.Created.ToString(),
                Choices = result.Completions.Select(c => new OpenAiChoice
                {
                    Text = c.Text.Trim(),
                    Index = c.Index,
                    FinishReason = c.FinishReason
                }).ToArray()
            };
        }


    }
}
