using ESBackupServer.App.Objects.Config;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.App.Objects.Factories.Config
{
    public class CommandDefinitionFactory
    {
        public List<CommandDefinition> Create(BackupTemplate template)
        {
            BackupTemplateSettingTypeRepository settingtyperepo = BackupTemplateSettingTypeRepository.GetInstance();
            List<CommandDefinition> list = new List<CommandDefinition>();
            foreach (BackupTemplateSetting item in template.Settings.Where(x => x.ActionType == null && settingtyperepo.Find(x.IDSettingType).Name != SettingTypeNames.Email))
            {
                list.Add(new CommandDefinition()
                {
                    Value = item.Value,
                    CommandType = settingtyperepo.Find(item.IDSettingType).Name
                });
            }
            return list;
        }

        public List<BackupTemplateSetting> Save(BackupTemplate template)
        {
            //TODO: Implement
            throw new System.NotImplementedException();
        }
    }
}