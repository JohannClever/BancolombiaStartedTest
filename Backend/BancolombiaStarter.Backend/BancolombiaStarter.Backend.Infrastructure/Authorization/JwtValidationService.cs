using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BancolombiaStarter.Backend.Infrastructure.Authorization
{
    public class JwtValidationService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secretKey;

        public JwtValidationService(IConfiguration config)
        {
            _issuer = config["Jwt:Issuer"];
            _audience = config["Jwt:Audience"];
            _secretKey = config["Jwt:SecretKey"];
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch (SecurityTokenException)
            {
                return false;
            }
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
            };
        }
    }

}
