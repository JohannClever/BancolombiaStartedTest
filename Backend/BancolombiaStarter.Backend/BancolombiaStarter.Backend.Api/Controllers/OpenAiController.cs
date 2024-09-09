using Azure;
using BancolombiaStarter.Backend.Domain.Dto;
using BancolombiaStarter.Backend.Domain.Services;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BancolombiaStarter.Backend.Api.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAiController : ControllerBase
    {
        private readonly IOpenAiService _openAiService;
        private readonly IProjectService _projectService;

        public OpenAiController(IOpenAiService openAiService, IProjectService projectService)
        {
            _openAiService = openAiService;
            _projectService = projectService;
        }

        [HttpPost("askIA")]
        public async Task<IActionResult> GenerateSuggestions([FromBody] OpenAiRequest request)
        {
            var result = await _openAiService.AskToIaAsync(request.Prompt);

            return Ok(result);
        }

        [HttpPost("generate-suggestions")]
        public async Task<IActionResult> GenerateSuggestions([FromBody] OpenAiSuggestionProjectRequest request)
        {
            // Validar que se haya proporcionado al menos un ID en SuggestionProjectsId
            if (request.SuggestionProjectsId == null || !request.SuggestionProjectsId.Any())
            {
                return BadRequest("Debe proporcionar al menos un ID de proyecto para sugerencias.");
            }

            // Obtener los proyectos sugeridos
            var projects = await _projectService.GetAsync(x => request.SuggestionProjectsId.Any(y => y == x.Id));

            // Validar que se encontraron proyectos
            if (projects == null || !projects.Any())
            {
                return NotFound("No se encontraron proyectos con los IDs proporcionados.");
            }

            // Obtener el proyecto individual
            var project = (await _projectService.GetAsync(x => x.Id == request.ProjectId)).FirstOrDefault();

            // Validar que el proyecto individual exista
            if (project == null)
            {
                return NotFound($"No se encontró el proyecto con ID {request.ProjectId}.");
            }

            // Convertir los proyectos a JSON para pasarlos al servicio de OpenAI
            var jsonProjects = JsonConvert.SerializeObject(projects);
            var jsonProject = JsonConvert.SerializeObject(project);

            var prompt = $@"
                        Analyze the following JSON list of projects to understand their characteristics and details:

                        List of projects:
                        {jsonProjects}

                        Here is a new project that is missing some key details:

                        Project:
                        {jsonProject}

                        Based on the analysis of these projects, please suggest values for the missing fields 'Name', 'Description', and 'Goal' of the new project. 

                        Make sure to provide a compelling and relevant suggestion for each field. Ensure that 'Name' and 'Description' are written in Spanish, and that 'Goal' is in decimal format. 

                        Return the suggestions in the following JSON format:

                        {{
                            ""Name"": ""<nombre sugerido>"",
                            ""Description"": ""<descripción sugerida>"",
                            ""Goal"": <meta sugerida en formato decimal>
                        }}

                        The 'Name' should reflect the nature and appeal of the project, 'Description' should be engaging and descriptive, and 'Goal' should be realistic based on the project's context.
                    ";
            // Llamar al servicio de OpenAI para obtener las sugerencias
            var response = await _openAiService.AskToIaAsync(prompt);
            var json = response.Choices.FirstOrDefault()?.Text;
            ProjectsResponse result = null;
            if (json != null && json.Contains("\"Name\""))
            {
                result = JsonConvert.DeserializeObject<ProjectsResponse>(json);

            }
            // Retornar el resultado de las sugerencias
            return Ok(result);
        }


    }


}