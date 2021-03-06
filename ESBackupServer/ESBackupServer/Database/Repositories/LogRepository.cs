﻿using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class LogRepository : AbRepository<Log>
    {
        #region AbRepository
        protected override void Add(Log item)
        {
            this._Context.Logs.Add(item);
            this.SaveChanges();
        }
        internal override Log Find(object id)
        {
            return this._Context.Logs.Find(id);
        }
        internal override List<Log> FindAll()
        {
            return this._Context.Logs.ToList();
        }
        internal override void Remove(Log item)
        {
            this._Context.Logs.Remove(item);
            this.SaveChanges();
        }
        internal override void Update(Log item)
        {
            Log log = this.Find(item.ID);
            log.IDClient = item.IDClient;
            log.IDBackup = item.IDBackup;
            log.LogType = item.LogType;
            log.UTCTime = item.UTCTime;
            log.Value = item.Value;
            this.SaveChanges();
        }
        #endregion

        internal void Remove(Client item)
        {
            foreach (Log lg in this.FindByClientID(item.ID))
            {
                this.Remove(lg);
            }
        }
        internal List<Log> FindByBackupID(long ID)
        {
            return this._Context.Logs.Where(x => x.IDBackup == ID).ToList();
        }
        internal List<Log> FindByClientID(int ID)
        {
            return this._Context.Logs.Where(x => x.IDClient == ID).ToList();
        }
        internal void Create(Client client, string message, LogTypeNames type)
        {
            this.Add(new Log(client, DateTime.UtcNow, message, type));
        }
        internal void Create(Client client, BackupInfo backup, string message, LogTypeNames type)
        {
            this.Add(new Log(client, backup, DateTime.UtcNow, message, type));
        }
    }
}