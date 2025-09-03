using AuthenticationWebApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationWebApplication.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(ShopUser user, string role);
    }
}
