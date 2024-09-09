using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BancolombiaStarter.Backend.Domain.Dto;
using BancolombiaStarter.Backend.Infrastructure.Authorization;
using BancolombiaStarter.Backend.Infrastructure.Authorization.Entities;
using System.Security.Claims;

namespace BancolombiaStarter.Backend.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {

        private readonly ILogger<AuthorizationController> _logger;
        private readonly JwtService _jwtService;
        private readonly JwtValidationService _jwtValidationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthorizationController(
            ILogger<AuthorizationController> logger,
            JwtService jwtService,
            JwtValidationService jwtValidationService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _jwtService = jwtService;
            _jwtValidationService = jwtValidationService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto model)
        {

            var user = await _userManager.FindByNameAsync(model.Username);
            var isCorrectPass = await _userManager.CheckPasswordAsync(user, model.Password);
            if (user == null || !isCorrectPass)
            {
                return Unauthorized(new { Message = "Credenciales inválidas" });
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                var roles = await _userManager.GetRolesAsync(user);
                var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var jwtToken = _jwtService.GenerateToken(userId, model.Username, roles);

                if (_jwtValidationService.ValidateToken(jwtToken))
                {
                    return Ok(new { id= userId, Token = jwtToken, FullName = user.FullName, Rols = string.Join(", ", roles) });
                }
                else
                {
                    return Unauthorized(new { Message = "Token JWT inválido" });
                }
            }
            else
            {
                return Unauthorized(new { Message = "Inicio de sesión fallido" });
            }

        }

    }
}