using HospitalNew.DAL.Interfaces;
using HospitalNew.BLL.Interfaces;
using HospitalNew.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace HospitalNew.BLL.Services
{
    
    public class AuthService : IAuthSerice
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.GivenName,user.Name.ToString()),
                new Claim(ClaimTypes.Role,user.Role.ToString()),

            });

            var expire  = DateTime.UtcNow.AddDays(1);

            var key = _configuration["Jwt:Key"];

            var keyEncoded = Encoding.ASCII.GetBytes(key);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(keyEncoded), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor 
            { 
                Subject = claims,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = expire,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);


        }
    }
}
