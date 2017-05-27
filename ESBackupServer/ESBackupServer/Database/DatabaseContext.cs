﻿using ESBackupServer.Database.Objects;
using System.Data.Entity;

namespace ESBackupServer.Database
{
    internal class DatabaseContext : DbContext
    {
        #region Singleton
        private static DatabaseContext _Instance { get; set; }
        private DatabaseContext() : base("name=MSSQL")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }
        public static DatabaseContext GetInstance()
        {
            if (DatabaseContext._Instance == null)
                DatabaseContext._Instance = new DatabaseContext();

            return DatabaseContext._Instance;
        }
        #endregion

        #region DbSets
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<BackupInfo> Backups { get; set; }
        public DbSet<BackupTemplate> Templates { get; set; }
        public DbSet<BackupTemplatePath> TemplatesPaths { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Login> Logins { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }
    }
}