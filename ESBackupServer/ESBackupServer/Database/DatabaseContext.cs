using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database
{
    internal class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=MSSQL")
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Backup> Backups { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }
    }
}