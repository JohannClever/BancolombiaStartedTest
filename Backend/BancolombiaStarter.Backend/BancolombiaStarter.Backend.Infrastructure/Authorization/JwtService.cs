using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BancolombiaStarter.Backend.Infrastructure.Authorization
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public class JwtService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secretKey;

        public JwtService(IConfiguration config)
        {
            _issuer = config["Jwt:Issuer"];
            _audience = config["Jwt:Audience"];
            _secretKey = config["Jwt:SecretKey"];
        }

        public string GenerateToken(string userId, string username, IList<string> roles, int expirationMinutes = 150)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, username)
        };

            if (roles != null && roles.Any())
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _issuer,
                Audience = _audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
                return userIdClaim?.Value;
            }
            catch (Exception)
            {
                // Handle the exception as needed (e.g., log it, rethrow it, etc.)
                return null;
            }
        }
    }

}
