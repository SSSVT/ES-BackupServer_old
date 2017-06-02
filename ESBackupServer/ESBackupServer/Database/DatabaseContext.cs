using ESBackupServer.Database.Objects;
using System.Data.Entity;

namespace ESBackupServer.Database
{
    internal class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=MSSQL")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        #region DbSets
        public DbSet<SmtpConfiguration> SmtpConfiguration { get; set; }

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