using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.Database.Repositories
{
    internal class LogTypeRepository : AbRepository<LogType>
    {
        #region Singleton
        private static LogTypeRepository _Instance { get; set; }
        private LogTypeRepository()
        {
        }
        public static LogTypeRepository GetInstance()
        {
            if (LogTypeRepository._Instance == null)
                LogTypeRepository._Instance = new LogTypeRepository();

            return LogTypeRepository._Instance;
        }
        #endregion
        #region AbRepository
        internal override void Add(LogType item)
        {
            this._Context.LogTypes.Add(item);
            this._Context.SaveChanges();
        }
        internal override LogType FindByID(int id)
        {
            return this._Context.LogTypes.Find(id);
        }
        internal override void Remove(LogType item)
        {
            this._Context.LogTypes.Remove(item);
            this._Context.SaveChanges();
        }
        internal override void Update(LogType item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}