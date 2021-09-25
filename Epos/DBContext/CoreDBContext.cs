using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epos.DBContext
{
    public class CoreDBContext : DbContext
    {
        // Open Package Manager console
        // Add-Migration *name* -Context CoreDBContext

        public CoreDBContext()
        {
        }
        
        public static DBContext.CoreDBContext Instance { get; } = CreateNewInstance();
        private static DBContext.CoreDBContext CreateNewInstance() { if (!System.IO.Directory.Exists("DB")) { System.IO.Directory.CreateDirectory("DB"); } return new CoreDBContext(); }

        public DbSet<Models.Conpany> Conpanys { get; set; }
        public DbSet<Models.Orders> Orders { get; set; }
        public DbSet<Models.ProductForCart> ProductForCart { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=DB/Core.db");
    }
}
