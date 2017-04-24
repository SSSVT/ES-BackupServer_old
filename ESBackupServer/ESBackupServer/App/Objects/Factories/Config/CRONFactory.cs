using ESBackupServer.App.Objects.Config;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.App.Objects.Factories.Config
{
    public class CRONFactory
    {
        public List<CRONDefinition> Create(BackupTemplate template, bool withEmail)
        {
            BackupTemplateSettingTypeRepository settingtyperepo = BackupTemplateSettingTypeRepository.GetInstance();
            List<CRONDefinition> list = new List<CRONDefinition>();
            if (withEmail)
            {
                foreach (BackupTemplateSetting item in template.Settings.Where(x => x.ActionType == true))
                {
                    list.Add(new CRONDefinition()
                    {
                        Value = item.Value,
                        CRON = item.CRON,
                        CommandType = settingtyperepo.Find(item.IDSettingType).Name
                    });
                }
            }
            else
            {
                foreach (BackupTemplateSetting item in template.Settings.Where(x => x.ActionType == true && settingtyperepo.Find(x.IDSettingType).Name != SettingTypeNames.Email))
                {
                    list.Add(new CRONDefinition()
                    {
                        Value = item.Value,
                        CRON = item.CRON,
                        CommandType = settingtyperepo.Find(item.IDSettingType).Name
                    });
                }
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