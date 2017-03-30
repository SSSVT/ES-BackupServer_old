using ESBackupServer.App.Objects.Config;
using ESBackupServer.Database.Objects;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.App.Objects.Factories
{
    public class CommandDefinitionFactory
    {
        public List<CommandDefinition> Create(BackupTemplate template)
        {
            List<CommandDefinition> list = new List<CommandDefinition>();
            foreach (BackupTemplateSetting item in template.Settings.Where(x => x.ActionType == null))
            {
                list.Add(new CommandDefinition()
                {
                    Value = item.Value
                });
            }
            return list;
        }
    }
}