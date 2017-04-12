using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;

namespace ESBackupServer.App.Objects.Factories.Config
{
    internal class ConfigurationFactory
    {
        private BackupTemplateSettingRepository _BackupTemplateRepo { get; set; } = BackupTemplateSettingRepository.GetInstance();

        internal Configuration Create(Client client)
        {
            EventDefinitionFactory EventFactory = new EventDefinitionFactory();
            CRONFactory TAFactory = new CRONFactory();

            Configuration config = new Configuration()
            {
                Templates = BackupTemplateRepository.GetInstance().Find(client)
            };

            foreach (BackupTemplate item in config.Templates)
            {
                item.Events = new EventDefinitionFactory().Create(item);
                item.TimeActions = new CRONFactory().Create(item);
                item.Commands = new CommandDefinitionFactory().Create(item);
            }

            return config;
        }

        internal bool Save(Configuration config)
        {
            foreach (BackupTemplate item in config.Templates)
            {
                //events
                //timeactions
                //commands
            }
            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}