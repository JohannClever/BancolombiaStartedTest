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
                    MaxTokens = 200, // Ajusta si necesitas más tokens
                    Temperature = 0.0, // Establecer temperatura baja para resultados consistentes
                    TopP = 0.5, // Limita las posibles palabras para mayor consistencia
                    FrequencyPenalty = 0.0,
                    PresencePenalty = 0.0
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
            catch (HttpRequestException ex) // Para manejar errores de conexión
            {
                throw new ApplicationException("Error de red al solicitar datos a OpenAI", ex);
            }
            catch (Exception ex)
            {
                // Manejo de errores general
                throw new ApplicationException("Error al solicitar datos a OpenAI", ex);
            }
        }

    }
}
