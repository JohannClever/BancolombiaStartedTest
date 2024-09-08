using BancolombiaStarter.Backend.Domain.Dto;
using BancolombiaStarter.Backend.Domain.Services;
using BancolombiaStarter.Backend.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace BancolombiaStarter.Backend.Api.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAiController : ControllerBase
    {
        private readonly IOpenAiService _openAiService;

        public OpenAiController(IOpenAiService openAiService)
        {
            _openAiService = openAiService;
        }

        [HttpPost("generate-suggestions")]
        public async Task<IActionResult> GenerateSuggestions([FromBody] OpenAiRequest request)
        {
            var result = await _openAiService.GetCampaignSuggestionsAsync(request.Prompt);

            return Ok(result);
        }

    }


}