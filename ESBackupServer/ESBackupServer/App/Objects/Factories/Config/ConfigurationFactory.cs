using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;

namespace ESBackupServer.App.Objects.Factories.Config
{
    internal class ConfigurationFactory
    {
        internal Configuration Create(Client client)
        {
            BackupTemplatePathRepository PathRepo = new BackupTemplatePathRepository();

            Configuration config = new Configuration()
            {
                Templates = new BackupTemplateRepository().Find(client)
            };
            foreach (BackupTemplate item in config.Templates)
            {
                item.Paths = PathRepo.Find(item);
            }
            config.StatusReportEnabled = client.StatusReportEnabled;
            config.ReportInterval = client.ReportInterval;            
            return config;
        }
    }
}