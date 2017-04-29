﻿using ESBackupServer.Database.Objects;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupRepository : AbRepository<Backup>
    {
        #region Singleton
        private BackupRepository()
        {

        }
        private static BackupRepository _Instance { get; set; }
        internal static BackupRepository GetInstance()
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
            this.SaveChanges();
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
            if (item.Status == 0 || item.Status == 2) //FULL (0) -> remove diffs; INCREMENTAL (2) -> remove next incremental backups
            {
                foreach (Backup backup in this.FindByBaseBackup(item))
                {
                    this.Remove(backup);
                }
            }
            else //DIFF (1)
            {
                this._Context.Backups.Remove(item);
            }            
            this.SaveChanges();
        }
        internal override void Update(Backup item)
        {
            Backup backup = this.Find(item.ID);
            if (backup == null)
            {
                this.Add(item);
            }
            else
            {
                backup.IDClient = item.IDClient;
                backup.IDBackupTemplate = item.IDBackupTemplate;
                backup.Name = item.Name;
                backup.Description = item.Description;
                backup.BackupType = item.BackupType;
                backup.BaseBackupID = item.BaseBackupID;
                backup.Source = item.Source;
                backup.Destination = item.Destination;
                backup.UTCExpiration = item.UTCExpiration;
                backup.Compressed = item.Compressed;
                backup.UTCStart = item.UTCStart;
                backup.UTCEnd = item.UTCEnd;
                backup.Status = item.Status;
                backup.PathOrder = item.PathOrder;
                backup.EmailSent = item.EmailSent;
                this.SaveChanges();
            }            
        }
        #endregion
        
        internal List<Backup> FindByClientID(int ID)
        {
            return this._Context.Backups.Where(x => x.IDClient == ID).ToList();
        }
        internal void Remove(long id)
        {
            this.Remove(this.Find(id));
        }
        private List<Backup> FindByBaseBackup(Backup item)
        {
            return this._Context.Backups.Where(x => x.BaseBackupID == item.ID).ToList();
        }
    }
}