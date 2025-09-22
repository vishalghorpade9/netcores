using Microsoft.EntityFrameworkCore;
using opc_ua_worker_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace opc_ua_worker_service.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DatabaseContext()
        {
        }       

        public DbSet<Machines> Machines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer("Server=CTHPGK3;Database=sai;Trusted_Connection=true;TrustServerCertificate=true");

            optionsBuilder.UseSqlServer("Server=CTHPGK3;User ID=sa; Password=admin@123;Database=sai;Trusted_Connection=false;TrustServerCertificate=true");
        }
    }
}
