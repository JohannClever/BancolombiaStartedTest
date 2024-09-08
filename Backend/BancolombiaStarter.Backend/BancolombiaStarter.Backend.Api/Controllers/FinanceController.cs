using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BancolombiaStarter.Backend.Domain.Dto;
using BancolombiaStarter.Backend.Domain.Entities;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;

namespace BancolombiaStarter.Backend.Api.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [ApiController]
    [Route("[controller]")]
    public class FinanceController : ControllerBase
    {

        private readonly ILogger<FinanceController> _logger;
        private IFinanceService _financeService;
        private readonly IMapper _mapper;

        public FinanceController(
            IFinanceService financeService,
            IMapper mapper)
        {
            _financeService = financeService;
            _mapper = mapper;
        }

        [HttpGet("GetAllFinance")]
        public async Task<IActionResult> GetAllFinanceAsync()
        {
            try
            {

                var result = await _financeService.GetAsync();
                var response = _mapper.Map<List<FinanceDto>>(result);
                return Ok(response);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }

        }

        [HttpDelete("DeleteFinance/{Id}")]
        public async Task<IActionResult> DeleteFinanceAsync([FromRoute] DeleteFinanceDto deleteDto)
        {
            FinanceDeleteResponseDto statusDeleteResponseDto;
            try
            {
                var result = await _financeService.DeleteAsync(deleteDto.Id);
                string message = result ? "Se eliminado el estado exitosamente." : "No se ha eliminado el estado.";
                statusDeleteResponseDto = new FinanceDeleteResponseDto()
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
                statusDeleteResponseDto = new FinanceDeleteResponseDto()
                {
                    Message = "Ocurrio un error eliminando el estado.",
                    Result = false
                };
                return BadRequest(statusDeleteResponseDto);
            }
        }

        [HttpPut("PutFinance")]
        public async Task<IActionResult> PutFinanceAsync([FromBody] FinanceUpdateDto updateDto)
        {
            FinanceUpdateResponseDto statusDeleteResponseDto;
            try
            {
                var entity = (await _financeService.GetAsync(filter: x => x.Id == updateDto.Id)).FirstOrDefault();
                if (entity == null)
                {
                    statusDeleteResponseDto = new FinanceUpdateResponseDto()
                    {
                        Message = "Error el Financiamiento a actualizar no existe.",
                        Result = false
                    };
                    return BadRequest(statusDeleteResponseDto);
                }

                entity.Description = updateDto.Description;


                var result = await _financeService.UpdateAsync(entity);
                string message = result ? "Se actualizó el Financiamiento exitosamente." : "No se ha actualizado el Financiamiento de los reportes.";
                statusDeleteResponseDto = new FinanceUpdateResponseDto()
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
                statusDeleteResponseDto = new FinanceUpdateResponseDto()
                {
                    Message = "Ocurrio un error actualizando el Financiamiento de los resportes.",
                    Result = false
                };
                return BadRequest(statusDeleteResponseDto);
            }

        }

        [HttpPost("PostFinance")]
        public async Task<IActionResult> PostFinanceAsync([FromBody] FinanceCreateDto createDto)
        {
            FinanceCreateResponseDto responseDto;
            try
            {
                var entity = _mapper.Map<Finance>(createDto);

                var entityId = (await _financeService.InsertAsync(entity));
                responseDto = new FinanceCreateResponseDto()
                {
                    Id = entityId,
                };
                return Ok(responseDto);


            }
            catch (Exception)
            {
                return BadRequest("Ocurrio un error al crear el estado.");
            }

        }

    }
}