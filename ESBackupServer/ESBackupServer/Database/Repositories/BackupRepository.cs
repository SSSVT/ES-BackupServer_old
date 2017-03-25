using ESBackupServer.Database.Objects;
using System;

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
        internal override void Add(Backup item)
        {
            this._Context.Backups.Add(item);
        }
        internal override Backup FindByID(int id)
        {
            return this._Context.Backups.Find(id);
        }
        internal override void Remove(Backup item)
        {
            this._Context.Backups.Remove(item);
        }
        internal override void Update(Backup item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}