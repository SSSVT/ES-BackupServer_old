using ESBackupServer.Database.Objects;
using System.Data.Entity;

namespace ESBackupServer.Database
{
    internal class DatabaseContext : DbContext
    {
        #region Singleton
        private static DatabaseContext _instance;
        private DatabaseContext() : base("name=MSSQL")
        {
        }
        public static DatabaseContext GetInstance()
        {
            if (DatabaseContext._instance == null)
                DatabaseContext._instance = new DatabaseContext();

            return DatabaseContext._instance;
        }
        #endregion

        public DbSet<Backup> Backups { get; set; }
        public DbSet<BackupDetail> BackupsDetails { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<LogType> LogTypes { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SettingType> SettingsTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }
    }
}