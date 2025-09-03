using AuthenticationWebApplication.Data;
using AuthenticationWebApplication.Models;
using AuthenticationWebApplication.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AuthenticationWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDatabaseContext authDatabaseContext;
        private readonly ITokenRepository tokenRepository;

        public AuthController(AuthDatabaseContext authDatabaseContext, ITokenRepository tokenRepository)
        {
            this.authDatabaseContext = authDatabaseContext;
            this.tokenRepository = tokenRepository;
        }


        [HttpPost]
        public async Task<IActionResult> ValidateUser([FromBody] Login login)
        {
            ShopUser? shopUser = authDatabaseContext.ShopUsers.Where(p => p.UserName == login.UserName && p.Password == login.Password && p.IsActive == true).FirstOrDefault();
            if (shopUser != null)
            {
                // CREATE JWT TOKEN
                var jwtToken = tokenRepository.CreateJWTToken(shopUser, "operator");
                var response = new LoginResponseDto
                {
                    JWTToken = jwtToken,
                    shopUser = shopUser
                };
                return Ok(response);
            }

            return Ok();
        }
    }
}
