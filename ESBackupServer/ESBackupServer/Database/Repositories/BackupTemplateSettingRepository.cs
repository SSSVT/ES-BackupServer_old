using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupTemplateSettingRepository : AbRepository<BackupTemplateSetting>
    {
        #region Singleton
        private BackupTemplateSettingRepository()
        {

        }
        private static BackupTemplateSettingRepository _Instance { get; set; }
        internal static BackupTemplateSettingRepository GetInstance()
        {
            if (BackupTemplateSettingRepository._Instance == null)
                BackupTemplateSettingRepository._Instance = new BackupTemplateSettingRepository();
            return BackupTemplateSettingRepository._Instance;
        }
        #endregion
        #region AbRepository
        protected override void Add(BackupTemplateSetting item)
        {
            this._Context.Settings.Add(item);
            this.SaveChanges();
        }
        internal override BackupTemplateSetting Find(object id)
        {
            return this._Context.Settings.Find(id);
        }
        internal override List<BackupTemplateSetting> FindAll()
        {
            return this._Context.Settings.ToList();
        }
        internal override void Remove(BackupTemplateSetting item)
        {
            this._Context.Settings.Remove(item);
            this.SaveChanges();
        }

        internal override void Update(BackupTemplateSetting item)
        {
            BackupTemplateSetting setting = this.Find(item.ID);
            setting.IDTemplate = item.IDTemplate;
            setting.IDSettingType = item.IDSettingType;
            setting.ActionType = item.ActionType;
            setting.Event = item.Event;
            setting.Time = item.Time;
            setting.Value = item.Value;
            this.SaveChanges();
        }
        #endregion

        internal List<BackupTemplateSetting> Find(BackupTemplate template)
        {
            return this._Context.Settings.Where(x => x.IDTemplate == template.ID).ToList();
        }
    }
}