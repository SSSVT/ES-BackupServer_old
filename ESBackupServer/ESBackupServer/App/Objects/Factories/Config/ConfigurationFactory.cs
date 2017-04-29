using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;

namespace ESBackupServer.App.Objects.Factories.Config
{
    internal class ConfigurationFactory
    {
        internal Configuration Create(Client client)
        {
            BackupTemplatePathRepository PathRepo = BackupTemplatePathRepository.GetInstance();

            Configuration config = new Configuration()
            {
                Templates = BackupTemplateRepository.GetInstance().Find(client)
            };
            foreach (BackupTemplate item in config.Templates)
            {
                item.Paths = PathRepo.Find(item);
            }
            return config;
        }
    }
}