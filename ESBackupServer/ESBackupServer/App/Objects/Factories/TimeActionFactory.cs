using ESBackupServer.App.Objects.Config;
using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.App.Objects.Factories
{
    public class TimeActionFactory
    {
        public List<TimeActionDefinition> Create(BackupTemplate template)
        {
            throw new NotImplementedException();

            List<TimeActionDefinition> list = new List<TimeActionDefinition>();
            foreach (BackupTemplateSetting item in template.Settings.Where(x => x.ActionType == true))
            {
                list.Add(new TimeActionDefinition()
                {
                    //TODO: Implement factory
                });
            }
            return list;
        }
    }
}