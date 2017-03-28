using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupRepository : AbRepository<Backup>
    {
        #region Singleton
        private static BackupRepository _Instance { get; set; }
        private BackupRepository()
        {
        }
        public static BackupRepository GetInstance()
        {
            if (BackupRepository._Instance == null)
                BackupRepository._Instance = new BackupRepository();

            return BackupRepository._Instance;
        }
        #endregion
        #region AbRepository
        protected override void Add(Backup item)
        {
            this._Context.Backups.Add(item);
            this._Context.SaveChanges();
        }
        internal override Backup Find(object id)
        {
            return this._Context.Backups.Find(id);
        }
        internal override List<Backup> FindAll()
        {
            return this._Context.Backups.ToList();
        }
        internal override void Remove(Backup item)
        {
            this._Context.Backups.Remove(item);
            this._Context.SaveChanges();
        }
        internal override void Update(Backup item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}