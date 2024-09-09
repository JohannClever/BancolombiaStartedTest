using BancolombiaStarter.Backend.Domain.Dto;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;
using OpenAI_API;
using OpenAI_API.Completions;
using System.Linq;
using System.Threading.Tasks;

namespace BancolombiaStarter.Backend.Domain.Services
{
    public class OpenAiService : IOpenAiService
    {
        private readonly OpenAIAPI _api;

        public OpenAiService(string apiKey)
        {
            _api = new OpenAIAPI(apiKey);
        }

        public async Task<OpenAiResponse> AskToIaAsync(string prompt)
        {
            try
            {
                var result = await _api.Completions.CreateCompletionAsync(new CompletionRequest
                {
                    Prompt = prompt,
                    MaxTokens = 150,
                    Temperature = 0.7,
                    TopP = 1.0, // Agregado para mayor control de la generación
                    FrequencyPenalty = 0.0, // Agregado para evitar penalización por frecuencia
                    PresencePenalty = 0.0 // Agregado para evitar penalización por presencia
                });

                var firstCompletion = result.Completions.FirstOrDefault();

                if (firstCompletion == null)
                {
                    throw new ApplicationException("No se recibió una respuesta válida de OpenAI.");
                }

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
            catch (Exception ex)
            {
                // Manejo de errores apropiado
                throw new ApplicationException("Error al solicitar datos a OpenAI", ex);
            }
        }
    }
}
