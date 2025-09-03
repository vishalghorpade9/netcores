using Microsoft.EntityFrameworkCore;
using windows_authentication_examples.Models;

namespace windows_authentication_examples.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected DataContext()
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
