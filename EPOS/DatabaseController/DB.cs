using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Data.Entity.Internal.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Data.Entity.Migrations;

namespace EPOS.DB
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class DBContext : DbContext
    {
        private static int CurrentUser;
        private static DBContext _instance;

        public static DBContext Instance
        {
            get
            {
                if (_instance == null || _instance != null && CurrentUser != TillManager.Instance.CurrentUser.GetHashCode())
                {
                    if(_instance != null)
                    { _instance.Dispose(); }

                    _instance = new DBContext();
                    CurrentUser = TillManager.Instance.CurrentUser.GetHashCode();
                }
                return _instance;
            }
        }

        public DbSet<Models.Attribute> Attributes { get; set; }
        public DbSet<Models.MixAndMatch> MixAndMatchs { get; set; }
        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<Models.OrderedAttribute> OrderedAttributes { get; set; }
        public DbSet<Models.OrderedProduct> OrderedProducts { get; set; }
        public DbSet<Models.ProductMixAndMatch> ProductMixAndMatchs { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public int ProductsAI { get { return GetAI("products"); } }
        public DbSet<Models.SKU> SKUs { get; set; }
        public DbSet<Models.VersionControl> VersionControls { get; set; }
        public DbSet<Models.WarrantyType> WarrantyTypes { get; set; }
        public static string ConnectionString {
            get
            {
                if(TillManager.Instance == null || TillManager.Instance.CurrentUser == null)
                {
                    return "server=127.0.0.1;port=3306;database=test;uid=root;password=456goldie@A1";
                }
                else
                {
                    return "server=" + TillManager.Instance.CurrentUser.DBURL + ";port=3306;database=" + TillManager.Instance.CurrentUser.DBName + ";uid=" + TillManager.Instance.CurrentUser.DBUsername + ";password=" + TillManager.Instance.CurrentUser.DBPassword;
                }
            }
        }

        private int GetAI(string Table)
        {
            return this.Database.SqlQuery<int>("select `AUTO_INCREMENT` from information_schema.TABLES  where TABLE_NAME = '" + Table + "' and TABLE_SCHEMA='" + TillManager.Instance.CurrentUser.DBName + "';").First();
        }
        public DBContext() : /*//*/base(ConnectionString)
        {
            //System.Data.Entity.Database.SetInitializer<DBContext>(new MyDbInitializer());
            System.Data.Entity.Database.SetInitializer<DBContext>(new MigrateDatabaseToLatestVersion<DBContext, Configuration>());
        }
    }

    public class MyDbInitializer : CreateDatabaseIfNotExists<DBContext>
    {
        protected override void Seed(DBContext context)
        {
            base.Seed(context);
        }
    }


    internal sealed class Configuration : DbMigrationsConfiguration<DBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
