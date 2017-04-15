using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupTemplateCommandRepository : AbRepository<BackupTemplateCommand>
    {
        #region Singleton
        private BackupTemplateCommandRepository()
        {

        }
        private static BackupTemplateCommandRepository _Instance { get; set; }
        internal static BackupTemplateCommandRepository GetInstance()
        {
            if (BackupTemplateCommandRepository._Instance == null)
                BackupTemplateCommandRepository._Instance = new BackupTemplateCommandRepository();
            return BackupTemplateCommandRepository._Instance;
        }
        #endregion
        #region AbRepository        
        protected override void Add(BackupTemplateCommand item)
        {
            throw new NotImplementedException();
        }

        internal override BackupTemplateCommand Find(object id)
        {
            throw new NotImplementedException();
        }

        internal override List<BackupTemplateCommand> FindAll()
        {
            return this._Context.TemplateCommands.ToList();
        }

        internal override void Remove(BackupTemplateCommand item)
        {
            throw new NotImplementedException();
        }

        internal override void Update(BackupTemplateCommand item)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}