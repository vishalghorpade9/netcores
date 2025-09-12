using Microsoft.EntityFrameworkCore;
using user_management_api.Model;

namespace user_management_api.Data
{
    public class UserManagementDbContext : DbContext
    {
        public UserManagementDbContext(DbContextOptions options) : base(options)
        {
        }      

        public DbSet<User> Users { get; set; }
    }
}
