using AuthenticationWebApplication.Models;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationWebApplication.Repositories
{
    public class TokenRepoitory : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepoitory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CreateJWTToken(ShopUser user, string role)
        {
            // Create claims 
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.FirstName));

            claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var crendetials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: crendetials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
