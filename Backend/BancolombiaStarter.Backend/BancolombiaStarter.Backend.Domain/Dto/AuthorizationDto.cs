using System.ComponentModel.DataAnnotations;

namespace BancolombiaStarter.Backend.Domain.Dto
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class CreateUserDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Rol { get; set; }

    }
}
