using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupDetailRepository : AbRepository<BackupDetail>
    {
        #region Singleton
        private static BackupDetailRepository _Instance { get; set; }
        private BackupDetailRepository()
        {
        }
        public static BackupDetailRepository GetInstance()
        {
            if (BackupDetailRepository._Instance == null)
                BackupDetailRepository._Instance = new BackupDetailRepository();

            return BackupDetailRepository._Instance;
        }
        #endregion
        #region AbRepository
        internal override void Add(BackupDetail item)
        {
            this._Context.BackupsDetails.Add(item);
            this._Context.SaveChanges();
        }
        internal override BackupDetail FindByID(int id)
        {
            return this._Context.BackupsDetails.Find(id);
        }
        internal override void Remove(BackupDetail item)
        {
            this._Context.BackupsDetails.Remove(item);
            this._Context.SaveChanges();
        }
        internal override void Update(BackupDetail item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}