using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupSettingTypeRepository : AbRepository<BackupTemplateSettingType>
    {
        #region Singleton
        private BackupSettingTypeRepository()
        {

        }
        private static BackupSettingTypeRepository _Instance { get; set; }
        internal static BackupSettingTypeRepository GetInstance()
        {
            if (BackupSettingTypeRepository._Instance == null)
                BackupSettingTypeRepository._Instance = new BackupSettingTypeRepository();
            return BackupSettingTypeRepository._Instance;
        }
        #endregion
        #region AbRepository
        protected override void Add(BackupTemplateSettingType item)
        {
            this._Context.SettingsTypes.Add(item);
            this.SaveChanges();
        }
        internal override BackupTemplateSettingType Find(object id)
        {
            return this._Context.SettingsTypes.Find(id);
        }
        internal override List<BackupTemplateSettingType> FindAll()
        {
            return this._Context.SettingsTypes.ToList();
        }
        internal override void Remove(BackupTemplateSettingType item)
        {
            this._Context.SettingsTypes.Remove(item);
            this.SaveChanges();
        }
        internal override void Update(BackupTemplateSettingType item)
        {
            BackupTemplateSettingType type = this.Find(item.ID);
            type.Name = item.Name;
            this.SaveChanges();
        }
        #endregion
    }
}