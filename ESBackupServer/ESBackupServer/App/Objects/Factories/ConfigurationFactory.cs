using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;

namespace ESBackupServer.App.Objects.Factories
{
    internal class ConfigurationFactory
    {
        internal Configuration Create(Client client)
        {
            EventDefinitionFactory EventFactory = new EventDefinitionFactory();
            TimeActionFactory TAFactory = new TimeActionFactory();

            Configuration config = new Configuration()
            {
                Client = client,
                Templates = BackupTemplateRepository.GetInstance().Find(client)
            };

            foreach (BackupTemplate item in config.Templates)
            {
                item.Events = new EventDefinitionFactory().Create(item);
                item.TimeActions = new TimeActionFactory().Create(item);
                item.Commands = new CommandDefinitionFactory().Create(item);
            }

            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}