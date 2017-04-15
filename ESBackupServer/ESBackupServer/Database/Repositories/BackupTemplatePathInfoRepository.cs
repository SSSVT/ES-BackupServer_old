using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupTemplatePathInfoRepository : AbRepository<BackupTemplatePathInfo>
    {
        #region Singleton
        private BackupTemplatePathInfoRepository()
        {

        }
        private static BackupTemplatePathInfoRepository _Instance { get; set; }
        internal static BackupTemplatePathInfoRepository GetInstance()
        {
            if (BackupTemplatePathInfoRepository._Instance == null)
                BackupTemplatePathInfoRepository._Instance = new BackupTemplatePathInfoRepository();
            return BackupTemplatePathInfoRepository._Instance;
        }
        #endregion
        #region AbRepository        
        protected override void Add(BackupTemplatePathInfo item)
        {
            throw new NotImplementedException();
        }

        internal override BackupTemplatePathInfo Find(object id)
        {
            throw new NotImplementedException();
        }

        internal override List<BackupTemplatePathInfo> FindAll()
        {
            return this._Context.TemplatePathInfo.ToList();
        }

        internal override void Remove(BackupTemplatePathInfo item)
        {
            throw new NotImplementedException();
        }

        internal override void Update(BackupTemplatePathInfo item)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}