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
        }
        public static DatabaseContext GetInstance()
        {
            if (DatabaseContext._Instance == null)
                DatabaseContext._Instance = new DatabaseContext();

            return DatabaseContext._Instance;
        }
        #endregion

        public DbSet<Backup> Backups { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<LogType> LogTypes { get; set; }
        public DbSet<SettingType> SettingsTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }
    }
}