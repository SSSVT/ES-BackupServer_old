using ESBackupServer.App.Objects.Config;
using ESBackupServer.Database.Objects;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.App.Objects.Factories.Config
{
    public class EventDefinitionFactory
    {
        public List<EventDefinition> Create(BackupTemplate template)
        {
            //TODO: remake
            List<EventDefinition> list = new List<EventDefinition>();
            foreach (BackupTemplateSetting item in template.Settings.Where(x => x.ActionType == false))
            {
                list.Add(new EventDefinition()
                {
                    IsBeforeEvent = (item.Event == false) ? true : false,
                    Value = item.Value
                });
            }
            return list;
        }
    }
}