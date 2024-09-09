using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BancolombiaStarter.Backend.Domain.Dto;
using BancolombiaStarter.Backend.Domain.Entities;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;
using BancolombiaStarter.Backend.Infrastructure.Authorization;
using BancolombiaStarter.Backend.Api.Extension;

namespace BancolombiaStarter.Backend.Api.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;
        private IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;

        public ProjectController(
            IProjectService projectService,
            IMapper mapper,
            JwtService jwtService)
        {
            _projectService = projectService;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [HttpGet("GetAllProjects")]
        public async Task<IActionResult> GetAllProjectsAsync()
        {
            try
            {
                var result = await _projectService.GetAsync();
                var response = _mapper.Map<List<ProjectsDto>>(result);
                return Ok(response);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpDelete("DeleteProjects/{Id}")]
        public async Task<IActionResult> DeleteProjectsAsync([FromRoute] DeleteProjectDto deleteProjectsDto)
        {
            ProjectsDeleteResponseDto statusDeleteResponseDto;
            try
            {
                var result = await _projectService.DeleteAsync(deleteProjectsDto.Id);
                string message = result ? "Se eliminó el proyecto." : "No se ha eliminado el proyecto.";
                statusDeleteResponseDto = new ProjectsDeleteResponseDto()
                {
                    Message = message,
                    Result = result
                };
                if (result)
                    return Ok(statusDeleteResponseDto);
                else
                    return BadRequest(statusDeleteResponseDto);
            }
            catch (Exception)
            {
                statusDeleteResponseDto = new ProjectsDeleteResponseDto()
                {
                    Message = "Ocurrió un error eliminando el proyecto.",
                    Result = false
                };
                return BadRequest(statusDeleteResponseDto);
            }
        }

        [HttpPut("PutProjects")]
        public async Task<IActionResult> PutProjectsAsync([FromBody] ProjectsUpdateDto updateDto)
        {
            ProjectsUpdateResponseDto statusDeleteResponseDto;
            try
            {
                var entity = (await _projectService.GetAsync(filter: x => x.Id == updateDto.Id)).FirstOrDefault();
                if (entity == null)
                {
                    statusDeleteResponseDto = new ProjectsUpdateResponseDto()
                    {
                        Message = "Error: el proyecto a actualizar no existe.",
                        Result = false
                    };
                    return BadRequest(statusDeleteResponseDto);
                }

                entity.Description = updateDto.Description;
                entity.Name = updateDto.Name;

                if(updateDto.Goal.HasValue)
                    entity.Goal = updateDto.Goal.Value;

                var result = await _projectService.UpdateAsync(entity);
                string message = result ? "Se actualizó el proyecto exitosamente." : "No se ha actualizado el proyecto.";
                statusDeleteResponseDto = new ProjectsUpdateResponseDto()
                {
                    Message = message,
                    Result = result
                };
                if (result)
                    return Ok(statusDeleteResponseDto);
                else
                    return BadRequest(statusDeleteResponseDto);
            }
            catch (Exception)
            {
                statusDeleteResponseDto = new ProjectsUpdateResponseDto()
                {
                    Message = "Ocurrió un error actualizando el proyecto.",
                    Result = false
                };
                return BadRequest(statusDeleteResponseDto);
            }
        }

        [HttpPost("PostProjects")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostProjectsAsync([FromForm] ProjectsCreateDto createDto)
        {
            ProjectsCreateResponseDto responseDto;
            try
            {
                var userId = HttpContext.GetUserIdFromToken(_jwtService);

                if (userId == null)
                {
                    return Unauthorized("Invalid token.");
                }

                var entity = _mapper.Map<Projects>(createDto);
                entity.UserId = userId;
                var entityId = (await _projectService.InsertAsync(entity, createDto.Picture));
                responseDto = new ProjectsCreateResponseDto()
                {
                    Id = entityId,
                };
                return Ok(responseDto);
            }
            catch (Exception)
            {
                return BadRequest("Ocurrió un error al crear el proyecto.");
            }
        }

        [HttpGet("GetProjectSuggestions/{id}")]
        public async Task<IActionResult> GetProjectSuggestionsAsync([FromRoute] long id)
        {
            try
            {
                var projects = await _projectService.GetProjectsToSuggestions(id);

                // Mapeo a DTO si es necesario
                var projectsDto = _mapper.Map<List<ProjectsDto>>(projects);

                if (projectsDto == null || !projectsDto.Any())
                {
                    return NotFound(new { Message = "No se encontraron proyectos similares." });
                }

                return Ok(projectsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener las sugerencias de proyectos para el ID {id}: {ex.Message}");
                return BadRequest(new { Message = "Ocurrió un error al obtener las sugerencias de proyectos." });
            }
        }

    }
}
