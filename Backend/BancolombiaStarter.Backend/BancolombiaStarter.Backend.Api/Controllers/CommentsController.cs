using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BancolombiaStarter.Backend.Domain.Dto;
using BancolombiaStarter.Backend.Domain.Entities;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;
using BancolombiaStarter.Backend.Infrastructure.Authorization.Entities;
using System.Linq.Expressions;

namespace BancolombiaStarter.Backend.Api.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {

        private readonly ILogger<CommentsController> _logger;
        private ICommentsService _commentsService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(
            ICommentsService commentsService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _commentsService = commentsService;
            _mapper = mapper;
            _userManager = userManager;
        }

        private Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }
        private Expression<Func<Comments, bool>> AddFilter(CommentsFilterDto reportByFilter)
        {
            Expression<Func<Comments, bool>> filter = null;

            if (!string.IsNullOrEmpty(reportByFilter.IdUser) && reportByFilter.IdUser != "none")
            {
                Expression<Func<Comments, bool>> idUserFilter = x => x.IdUser == reportByFilter.IdUser;
                filter = filter == null ? idUserFilter : CombineExpressions(filter, idUserFilter);
            }

            if (reportByFilter.ProjectId != null && reportByFilter.ProjectId != 0)
            {
                Expression<Func<Comments, bool>> serviceIdFilter = x => x.ProjectId == reportByFilter.ProjectId;
                filter = filter == null ? serviceIdFilter : CombineExpressions(filter, serviceIdFilter);
            }
            return filter;
        }

        [HttpGet("GetResports/{ProjectId}/{IdUser}")]
        public async Task<IActionResult> GetResportsAsync([FromRoute] CommentsFilterDto reportByFilter)
        {
            try
            {

                var include = new Expression<Func<Comments, object>>[]
                        {
                                            p => p.Project,
                                            p => p.IdUser
                        };
                var filter = AddFilter(reportByFilter);
                var result = await _commentsService.GetAsync(
                                                     filter: filter,
                                                     includeObjectProperties: include);

                var reportUsers = result.Select(x => x.IdUser).Distinct().ToList();

                var users = _userManager.Users.Where(x => reportUsers.Contains(x.Id)).ToList();

                List<CommentsDto> response = new List<CommentsDto>();
                foreach (var item in result)
                {
                    var dto = _mapper.Map<CommentsDto>(item);
                    var user = users.FirstOrDefault(x => x.Id == item.IdUser);
                    dto.UserName = user != null ? user.UserName : string.Empty;
                    response.Add(dto);
                }

                return Ok(response);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }

        }

        [HttpDelete("DeleteComments/{Id}")]
        public async Task<IActionResult> DeleteCommentsAsync([FromRoute] CommentsDeleteDto deleteDto)
        {
            CommentsDeleteResponseDto reportDeleteResponseDto;
            try
            {
                var result = await _commentsService.DeleteAsync(deleteDto.Id);
                string message = result ? "Se eliminado el comentario exitosamente." : "No se ha eliminado el comentario.";
                reportDeleteResponseDto = new CommentsDeleteResponseDto()
                {
                    Message = message,
                    Result = result
                };
                if (result)
                    return Ok(reportDeleteResponseDto);
                else
                    return BadRequest(reportDeleteResponseDto);

            }
            catch (Exception)
            {
                reportDeleteResponseDto = new CommentsDeleteResponseDto()
                {
                    Message = "Ocurrio un error eliminando el comentario.",
                    Result = false
                };
                return BadRequest(reportDeleteResponseDto);
            }
        }

        [HttpPut("PutComments")]
        public async Task<IActionResult> PutCommentsAsync([FromBody] CommentsUpdateDto updateDto)
        {
            CommentsUpdateResponseDto reportDeleteResponseDto;
            try
            {
                var entity = (await _commentsService.GetAsync(filter: x => x.Id == updateDto.Id)).FirstOrDefault();
                if (entity == null)
                {
                    reportDeleteResponseDto = new CommentsUpdateResponseDto()
                    {
                        Message = "Error el comentario a actualizar no existe.",
                        Result = false
                    };
                    return BadRequest(reportDeleteResponseDto);
                }

                entity.Observations = updateDto.Observations;


                var result = await _commentsService.UpdateAsync(entity);
                string message = result ? "Se actualizó el comentario exitosamente." : "No se ha actualizado el comentario el comentario.";
                reportDeleteResponseDto = new CommentsUpdateResponseDto()
                {
                    Message = message,
                    Result = result
                };
                if (result)
                    return Ok(reportDeleteResponseDto);
                else
                    return BadRequest(reportDeleteResponseDto);

            }
            catch (Exception)
            {
                reportDeleteResponseDto = new CommentsUpdateResponseDto()
                {
                    Message = "Ocurrio un error actualizando el comentario.",
                    Result = false
                };
                return BadRequest(reportDeleteResponseDto);
            }

        }

        [HttpPost("PostComments")]
        public async Task<IActionResult> PostCommentsAsync([FromBody] CommentsCreateDto reportCreateDto)
        {
            CommentsCreateResponseDto reportCreateResponseDto;
            try
            {
                IdentityUser usuarioActual = await _userManager.GetUserAsync(User);

                if (usuarioActual == null)
                {
                    return BadRequest("Usuario no encontrado");
                }
                var entity = _mapper.Map<Comments>(reportCreateDto);
                entity.IdUser = string.IsNullOrEmpty(reportCreateDto.IdUser) ? usuarioActual.Id : reportCreateDto.IdUser;

                var entityId = (await _commentsService.InsertAsync(entity));
                reportCreateResponseDto = new CommentsCreateResponseDto()
                {
                    Id = entityId,
                };
                return Ok(reportCreateResponseDto);


            }
            catch (Exception)
            {
                return BadRequest("Ocurrio un error al crear el comentario.");
            }

        }

    }
}