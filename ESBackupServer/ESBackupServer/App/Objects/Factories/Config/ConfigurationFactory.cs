using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;

namespace ESBackupServer.App.Objects.Factories.Config
{
    internal class ConfigurationFactory
    {
        internal Configuration Create(Client client)
        {
            Configuration config = new Configuration()
            {
                Templates = BackupTemplateRepository.GetInstance().Find(client)
            };
            return config;
        }

        internal void Save(Configuration config)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}