using AuthenticationWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationWebApplication.Data
{
    public class AuthDatabaseContext : DbContext
    {
        public AuthDatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected AuthDatabaseContext()
        {
        }

        public DbSet<ShopUser> ShopUsers { get; set; }
    }
}
