using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BancolombiaStarter.Backend.Domain.Dto;
using BancolombiaStarter.Backend.Infrastructure.Authorization;
using BancolombiaStarter.Backend.Infrastructure.Authorization.Entities;
using System.Security.Claims;

namespace BancolombiaStarter.Backend.Api.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<AuthorizationController> _logger;
        private readonly JwtService _jwtService;
        private readonly JwtValidationService _jwtValidationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(
            ILogger<AuthorizationController> logger,
            JwtService jwtService,
            JwtValidationService jwtValidationService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            _logger = logger;
            _jwtService = jwtService;
            _jwtValidationService = jwtValidationService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }


        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto model)
        {

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null )
            {
                return Unauthorized(new { Message = "El usuario ya existe." });
            }
             user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName
             };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Rol);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
           

        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {

            List<ApplicationUser> users = _userManager.Users.ToList();

            if (users.Any())
            {
                var response = _mapper.Map<List<UserDto>>(users);
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet("GetUsers/{userId}")]
        public async Task<IActionResult> GetUsers(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var response = _mapper.Map<UserDto>(user);
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }

        }


        [HttpGet("protected-resource")]
        [Authorize]
        public IActionResult GetProtectedResource()
        {
            // Este método solo es accesible para usuarios autenticados con un token JWT válido
            return Ok(new { Message = "Este es un recurso protegido" });
        }
    }
}