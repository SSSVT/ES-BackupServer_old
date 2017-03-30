using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class LogTypeRepository : AbRepository<LogType>
    {
        #region Singleton
        private LogTypeRepository()
        {

        }
        private static LogTypeRepository _Instance { get; set; }
        internal static LogTypeRepository GetInstance()
        {
            if (LogTypeRepository._Instance == null)
                LogTypeRepository._Instance = new LogTypeRepository();
            return LogTypeRepository._Instance;
        }
        #endregion
        #region AbRepository
        protected override void Add(LogType item)
        {
            this._Context.LogTypes.Add(item);
            this._Context.SaveChanges();
        }
        internal override LogType Find(object id)
        {
            return this._Context.LogTypes.Find(id);
        }
        internal override List<LogType> FindAll()
        {
            return this._Context.LogTypes.ToList();
        }
        internal override void Remove(LogType item)
        {
            this._Context.LogTypes.Remove(item);
            this._Context.SaveChanges();
        }
        internal override void Update(LogType item)
        {
            LogType type = this.Find(item.ID);
            type.Name = item.Name;
            type.Logs = item.Logs;
            this._Context.SaveChanges();
        }
        #endregion
    }
}