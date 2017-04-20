using ESBackupServer.App.Objects.Config;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.App.Objects.Factories.Config
{
    public class EventDefinitionFactory
    {
        public List<EventDefinition> Create(BackupTemplate template)
        {
            BackupTemplateSettingTypeRepository settingtyperepo = BackupTemplateSettingTypeRepository.GetInstance();
            List<EventDefinition> list = new List<EventDefinition>();
            foreach (BackupTemplateSetting item in template.Settings.Where(x => x.ActionType == false && settingtyperepo.Find(x.IDSettingType).Name != SettingTypeNames.Email))
            {
                list.Add(new EventDefinition()
                {
                    IsBeforeEvent = (item.Event == false) ? true : false,
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