using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.Database.Repositories
{
    internal class LogRepository : AbRepository<Log>
    {
        #region Singleton
        private static LogRepository _Instance { get; set; }
        private LogRepository()
        {
        }
        public static LogRepository GetInstance()
        {
            if (LogRepository._Instance == null)
                LogRepository._Instance = new LogRepository();

            return LogRepository._Instance;
        }
        #endregion
        #region AbRepository
        protected override void Add(Log item)
        {
            this._Context.Logs.Add(item);
            this._Context.SaveChanges();
        }
        internal override Log Find(object id)
        {
            return this._Context.Logs.Find(id);
        }
        internal override void Remove(Log item)
        {
            this._Context.Logs.Remove(item);
            this._Context.SaveChanges();
        }
        internal override void Update(Log item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion

        internal void Create(Client client, string message, LogTypeNames type)
        {
            this.Add(new Log(client, DateTime.UtcNow, message, type));
        }
        internal void Create(Client client, Backup backup, string message, LogTypeNames type)
        {
            this.Add(new Log(client, backup, DateTime.UtcNow, message, type));
        }
    }
}