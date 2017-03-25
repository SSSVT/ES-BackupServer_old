using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.Database.Repositories
{
    internal class LogRepository : AbRepository<Log>
    {
        #region AbRepository
        internal override void Add(Log item)
        {
            this._Context.Logs.Add(item);
        }
        internal override Log FindByID(int id)
        {
            return this._Context.Logs.Find(id);
        }
        internal override void Remove(Log item)
        {
            this._Context.Logs.Remove(item);
        }
        internal override void Update(Log item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}