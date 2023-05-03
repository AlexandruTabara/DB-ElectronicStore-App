using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_ElectronicStore_App.Modules
{
    public class ElectronicStoreMgmtDB : DbContext
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<LabelProduct> LabelProduct { get; set; }
        public DbSet<Producer> Producer { get; set; }

        public ElectronicStoreMgmtDB()
        {
           this. Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\otiot\\source\\repos\\DB-ElectronicStore-App\\DB-ElectronicStore-App\\ElectronicStoreDB.mdf;Integrated Security=True");
        }
    }
}
