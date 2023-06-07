using EduSpaceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduSpaceAPI.Helpers
{
    public class JWTGenerator
    {
        private IConfiguration _configuration;

        public JWTGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string GetToken(UserModel model)
        {
            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]); // Replace with your own secret key

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, model.UserId.ToString()), // Add any additional claims as needed
                    new Claim(ClaimTypes.Email, model.Email!), // Add any additional claims as needed
                    new Claim(ClaimTypes.Role, model.UserRole!) // Add any additional claims as needed
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Set token expiration time
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }

    }
}
