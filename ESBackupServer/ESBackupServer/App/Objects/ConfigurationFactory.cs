using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESBackupServer.App.Objects
{
    internal class ConfigurationFactory
    {
        internal Configuration Create(Client client)
        {
            Configuration config = new Configuration()
            {
                Client = client,
                Templates = BackupTemplateRepository.GetInstance().Find(client),
                Events = null,
                TimeActions = null
            };

            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}