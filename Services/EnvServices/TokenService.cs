using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Sysachad.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(int legajo, bool isAdmin) {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, legajo.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("IsAdmin", isAdmin.ToString()),

            };
            var rsa = RSAKeyProvider.GetPrivateKey();
            var creds = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}