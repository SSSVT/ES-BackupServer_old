using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupTemplateTimeSettingRepository : AbRepository<BackupTemplateTimeSetting>
    {
        #region Singleton
        private BackupTemplateTimeSettingRepository()
        {

        }
        private static BackupTemplateTimeSettingRepository _Instance { get; set; }
        internal static BackupTemplateTimeSettingRepository GetInstance()
        {
            if (BackupTemplateTimeSettingRepository._Instance == null)
                BackupTemplateTimeSettingRepository._Instance = new BackupTemplateTimeSettingRepository();
            return BackupTemplateTimeSettingRepository._Instance;
        }
        #endregion
        #region AbRepository        
        protected override void Add(BackupTemplateTimeSetting item)
        {
            throw new NotImplementedException();
        }

        internal override BackupTemplateTimeSetting Find(object id)
        {
            throw new NotImplementedException();
        }

        internal override List<BackupTemplateTimeSetting> FindAll()
        {
            return this._Context.TemplateTimeSetting.ToList();
        }

        internal override void Remove(BackupTemplateTimeSetting item)
        {
            throw new NotImplementedException();
        }

        internal override void Update(BackupTemplateTimeSetting item)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}