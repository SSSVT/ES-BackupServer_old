using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupTemplateRepository : AbRepository<BackupTemplate>
    {
        #region Singleton
        private BackupTemplateRepository()
        {

        }
        private static BackupTemplateRepository _Instance { get; set; }
        internal static BackupTemplateRepository GetInstance()
        {
            if (BackupTemplateRepository._Instance == null)
                BackupTemplateRepository._Instance = new BackupTemplateRepository();
            return BackupTemplateRepository._Instance;
        }
        #endregion
        #region AbRepository
        protected override void Add(BackupTemplate item)
        {
            this._Context.Templates.Add(item);
            this._Context.SaveChanges();
        }
        internal override BackupTemplate Find(object id)
        {
            return this._Context.Templates.Find(id);
        }
        internal override List<BackupTemplate> FindAll()
        {
            return this._Context.Templates.ToList();
        }
        internal override void Remove(BackupTemplate item)
        {
            this._Context.Templates.Remove(item);
            this._Context.SaveChanges();
        }

        internal override void Update(BackupTemplate item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion

        internal List<BackupTemplate> Find(Client client)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}