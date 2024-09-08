using BancolombiaStarter.Backend.Domain.Services;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancolombiaStarter.Backend.Api.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [ApiController]
    [Route("api/[controller]")]
    public class KickstarterController : ControllerBase
    {
        private readonly IKickstarterService _kickstarterService;

        public KickstarterController(IKickstarterService kickstarterService)
        {
            _kickstarterService = kickstarterService;
        }

        // Endpoint para obtener información de un proyecto específico por su ID
        [HttpGet("project/{id}")]
        public async Task<IActionResult> GetProjectById(string id)
        {
            var project = await _kickstarterService.GetProjectByIdAsync(id);
            return Ok(project);
        }

        // Endpoint para buscar proyectos en Kickstarter por un término de búsqueda
        [HttpGet("search")]
        public async Task<IActionResult> SearchProjects([FromQuery] string term)
        {
            var projects = await _kickstarterService.SearchProjectsAsync(term);
            return Ok(projects);
        }
    }

}