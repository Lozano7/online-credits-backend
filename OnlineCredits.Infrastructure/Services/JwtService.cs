using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OnlineCredits.Core.Entities;
using Microsoft.Extensions.Configuration;
using System;

namespace OnlineCredits.Infrastructure.Services
{
    public class JwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly int _expireMinutes;

        public JwtService(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"] ?? "clave_super_secreta_para_jwt";
            _issuer = configuration["Jwt:Issuer"] ?? "OnlineCreditsIssuer";
            _expireMinutes = int.TryParse(configuration["Jwt:ExpireMinutes"], out var min) ? min : 60;
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("firstName", user.FirstName ?? ""),
                new Claim("lastName", user.LastName ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 