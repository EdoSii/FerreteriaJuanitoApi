using FerreteriaJuanitoApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FerreteriaJuanitoApi.Services
{
    public interface IJwtService
    {
        string GenerateToken(string id, string firstname, string lastname, string email, string phone, string gender, Rol rol);
    }
    public class JwtService : IJwtService
    {
        public string SecretKey { get; set; }
        public int TokenDuration { get; set; }
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            SecretKey = _configuration.GetSection("jwtConfig").GetSection("Key").Value;
            TokenDuration = int.Parse(_configuration.GetSection("jwtConfig").GetSection("Duration").Value);
        }

        public string GenerateToken(string id, string firstname, string lastname, string email, string phone, string gender, Rol rol)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var payload = new[]
            {
                new Claim("id", id),
                new Claim("firstname", firstname),
                new Claim("lastname", lastname),
                new Claim("email", email),
                new Claim("phone", phone),
                new Claim("gender", gender),
                new Claim(ClaimTypes.Role,rol.ToString())
            };

            var jwtToken = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: payload,
                expires: DateTime.UtcNow.AddMinutes(TokenDuration),
                signingCredentials: signature
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
