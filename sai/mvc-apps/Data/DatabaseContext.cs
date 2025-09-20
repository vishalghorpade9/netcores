using Microsoft.EntityFrameworkCore;
using mvc_apps.Models;

namespace mvc_apps.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected DatabaseContext()
        {
        }

        public  DbSet<ShopUser> ShopUsers { get; set; }
        public DbSet<Machines> Machines { get; set; } = default!;
    }
}
