using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.Database.Repositories
{
    internal class LogTypeRepository : AbRepository<LogType>
    {
        #region Singleton
        private static LogTypeRepository _instance;
        private LogTypeRepository()
        {
        }
        public static LogTypeRepository GetInstance()
        {
            if (LogTypeRepository._instance == null)
                LogTypeRepository._instance = new LogTypeRepository();

            return LogTypeRepository._instance;
        }
        #endregion
        #region AbRepository
        internal override void Add(LogType item)
        {
            this._Context.LogTypes.Add(item);
        }
        internal override LogType FindByID(int id)
        {
            return this._Context.LogTypes.Find(id);
        }
        internal override void Remove(LogType item)
        {
            this._Context.LogTypes.Remove(item);
        }
        internal override void Update(LogType item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}