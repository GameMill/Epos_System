using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epos.DBContext
{
    public class ProductsDBContext : DbContext
    {
        // Open Package Manager console
        // Add-Migration *name* -Context CoreDBContext

        public ProductsDBContext()
        {
        }

        public static DBContext.ProductsDBContext Instance { get; } = CreateNewInstance();
        private static DBContext.ProductsDBContext CreateNewInstance() { if (!System.IO.Directory.Exists("DB")) { System.IO.Directory.CreateDirectory("DB"); } return new ProductsDBContext(); }

        public DbSet<Models.Product> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=DB/Products.db");
    }
}
