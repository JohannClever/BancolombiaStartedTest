using BancolombiaStarter.Backend.Domain.Dto;
using BancolombiaStarter.Backend.Domain.Entities;
using BancolombiaStarter.Backend.Domain.Ports;
using BancolombiaStarter.Backend.Domain.Services.Generic;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace BancolombiaStarter.Backend.Domain.Services
{
    [DomainService]
    public class ProjectsService : IProjectService
    {
        private readonly IGenericRepository<Projects> _repository;
        private readonly IFileBlobStorageManager _storageManager;
        private readonly IConfiguration _configuration;
        private readonly IOpenAiService _openAiService;

        public ProjectsService(
            IGenericRepository<Projects> repository,
            IFileBlobStorageManager storageManager,
            IConfiguration configuration,
            IOpenAiService openAiService)
        {
            _repository = repository;
            _storageManager = storageManager;
            _configuration = configuration;
            _openAiService = openAiService;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = (await _repository.GetAsync(filter: x => x.Id == id)).FirstOrDefault();

            if (entity == null)
                return false;

            try
            {
                await _repository.DeleteAsync(entity);
                return true;

            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public async Task<List<Projects>> GetAsync(
            Expression<Func<Projects, bool>>? filter = null,
            Func<IQueryable<Projects>, IOrderedQueryable<Projects>>? orderBy = null,
            bool isTracking = false,
            params Expression<Func<Projects, object>>[] includeObjectProperties)
        {
            var query = await _repository.GetAsync(filter, orderBy, isTracking, includeObjectProperties);
            return query.ToList();
        }


        public async Task<List<Projects>> GetProjectsToSuggestions(long id)
        {
            var query = await _repository.GetAsync(filter: x => x.Id == id, isTracking: false);
            var project = query.FirstOrDefault();
            var result = new List<Projects>();

            if (project != null)
            {
                // Obtener todos los proyectos cuyo estado sea 'Financed'
                var queryProjects = await _repository.GetAsync(filter: x =>
                    x.Id != id && x.Status == ProjectStatus.Financed,
                    isTracking: false);

                // Ordenar los proyectos por la diferencia en días entre 'CreatedOn' y 'FinancedDate' de menor a mayor
                var orderedProjects = queryProjects
                    .Where(x => x.FinancedDate.HasValue) // Filtrar proyectos que tengan 'FinancedDate'
                    .OrderBy(x => (x.FinancedDate.Value - x.CreationOn).Days) // Ordenar por la diferencia en días
                    .ToList();

                // Procesar los proyectos en lotes de 10
                var projectsToCheck = new List<Projects>();
                foreach (var batch in orderedProjects.Select((value, index) => new { value, index })
                                                     .GroupBy(x => x.index / 10))
                {
                    // Crear un prompt para enviar a OpenAI
                    var batchProjects = batch.Select(x => x.value).ToList();
                    var prompt = GeneratePrompt(batchProjects, project);

                    // Consumir el servicio OpenAI para evaluar similitud
                    var response = await _openAiService.AskToIaAsync(prompt);

                    // Convertir la respuesta en una lista de proyectos similares
                    var similarProjects = ParseSimilarProjects(response, batchProjects);

                    // Agregar los proyectos similares a la lista final
                    projectsToCheck.AddRange(similarProjects);

                    // Si ya se tienen más de 20 proyectos, detener el procesamiento
                    if (projectsToCheck.Count >= 20)
                        break;
                }

                return projectsToCheck.Take(20).ToList();
            }

            return result;
        }

        // Método para generar el prompt para OpenAI
        private string GeneratePrompt(List<Projects> batchProjects, Projects baseProject)
        {
            var batchJson = JsonConvert.SerializeObject(batchProjects);
            var projectJson = JsonConvert.SerializeObject(baseProject);

            return $@"
                    Compare the following projects to the base project, and determine if they are discussing the same topic or something similar, check it has synonyms in the name and descriptions.

                    Base project:
                    {projectJson}

                    Projects to compare:
                    {batchJson}

                    Please return a list of IDs for projects that are similar to the base project in the following JSON format. The IDs should be in Spanish:

                    {{
                        ""SimilarProjectIds"": [
                            ""<id del proyecto similar>"",
                            ""<id del proyecto similar>"",
                            ...
                        ]
                    }}
                    Only include the IDs of the projects that are similar. The 'SimilarProjectIds' field should contain a list of IDs of the projects that are considered similar to the base project.";
        }


        // Método para parsear la respuesta de OpenAI y convertirla en una lista de proyectos similares
        private List<Projects> ParseSimilarProjects(OpenAiResponse response, List<Projects> batchProjects)
        {
            var similarProjectsJson = response.Choices.FirstOrDefault()?.Text;

            // Verifica si similarProjectsJson contiene la estructura JSON esperada
            if (similarProjectsJson != null && similarProjectsJson.Contains("\"SimilarProjectIds\""))
            {
                // Deserializa el JSON usando la estructura de la respuesta esperada
                var result = JsonConvert.DeserializeObject<SimilarProjectsResponse>(similarProjectsJson);

                // Obtén la lista de IDs similares
                var similarProjectIds = result?.SimilarProjectIds ?? new List<long>();

                // Filtra los proyectos de batchProjects basados en los IDs similares
                return batchProjects.Where(p => similarProjectIds.Contains(p.Id)).ToList();
            }
            else
            {
                // Si no contiene la estructura esperada, deserializa como lista de long
                var similarProjectIds = JsonConvert.DeserializeObject<List<long>>(similarProjectsJson);

                // Filtra los proyectos de batchProjects basados en los IDs similares
                return batchProjects.Where(p => similarProjectIds.Contains(p.Id)).ToList();
            }
        }

        public async Task<long> InsertAsync(Projects entity, IFormFile picture)
        {

            entity.PictureUrl = await UploadPictureAsync(picture);
            var result = await _repository.AddAsync(entity);
            if (result == null)
                throw new Exception("Error al insertar el proyecto.");

            return result.Id;
        }

        public async Task<bool> UpdateAsync(Projects entity)
        {
            try
            {
                await _repository.UpdateAsync(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<string> UploadPictureAsync(IFormFile picture)
        {
            string result = string.Empty;
            if (picture == null || picture.Length == 0)
            {
                return result;
            }

            try
            {

                result = await _storageManager.SaveFileAsync(_configuration["Storage:ImageContainer"], picture);

                return result;
            }
            catch (Exception exc)
            {
                var message = exc.InnerException == null ? exc.Message : exc.InnerException.Message;
                throw new Exception(message);
            }
        }
    }
}
