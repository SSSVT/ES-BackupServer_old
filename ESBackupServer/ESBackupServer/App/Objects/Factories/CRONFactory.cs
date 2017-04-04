using ESBackupServer.App.Objects.Config;
using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.App.Objects.Factories
{
    public class CRONFactory
    {
        public List<CRONDefinition> Create(BackupTemplate template)
        {
            List<CRONDefinition> list = new List<CRONDefinition>();
            foreach (BackupTemplateSetting item in template.Settings.Where(x => x.ActionType == true))
            {
                list.Add(new CRONDefinition()
                {
                    Value = item.Value
                });
            }
            return list;
        }
    }
}