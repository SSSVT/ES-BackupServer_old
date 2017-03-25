using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupDetailRepository : AbRepository<BackupDetail>
    {
        #region AbRepository
        internal override void Add(BackupDetail item)
        {
            this._Context.BackupsDetails.Add(item);
        }
        internal override BackupDetail FindByID(int id)
        {
            return this._Context.BackupsDetails.Find(id);
        }
        internal override void Remove(BackupDetail item)
        {
            this._Context.BackupsDetails.Remove(item);
        }
        internal override void Update(BackupDetail item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}