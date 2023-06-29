using API_Project.Interface;
using API_Project.Models;
using API_Project.Models.Manager;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Project.Services
{
    public class JwtToken:IJwtToken
    {
        private readonly IConfiguration _config;
        public JwtToken(IConfiguration config)
        {
            this._config = config;
        }

       //method for user to create token
        public string CreateToken(string email,string role)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, email));
            claims.Add(new Claim(ClaimTypes.Role, role));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
