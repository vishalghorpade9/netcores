using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCUA.Data
{
    public class OpcUaDatabaseContext : DbContext
    {
        public OpcUaDatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public OpcUaDatabaseContext()
        {
        }

        public DbSet<Machines> Machines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=CTHPGK3;Database=sai;Trusted_Connection=true;TrustServerCertificate=true");
        }
    }
}
