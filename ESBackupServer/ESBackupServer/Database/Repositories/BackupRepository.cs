using ESBackupServer.Database.Objects;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupRepository : AbRepository<BackupInfo>
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
        protected override void Add(BackupInfo item)
        {
            this._Context.Backups.Add(item);
            this.SaveChanges();
        }
        internal override BackupInfo Find(object id)
        {
            return this._Context.Backups.Find(id);
        }
        internal override List<BackupInfo> FindAll()
        {
            return this._Context.Backups.ToList();
        }
        internal override void Remove(BackupInfo item)
        {
            if (item.Status == 0 || item.Status == 2) //FULL (0) -> remove diffs; INCREMENTAL (2) -> remove next incremental backups
            {
                foreach (BackupInfo backup in this.FindByBaseBackup(item))
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
        internal override void Update(BackupInfo item)
        {
            BackupInfo backup = this.Find(item.ID);
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
        
        internal List<BackupInfo> FindByClientID(int ID)
        {
            return this._Context.Backups.Where(x => x.IDClient == ID).ToList();
        }
        internal void Remove(long id)
        {
            this.Remove(this.Find(id));
        }
        private List<BackupInfo> FindByBaseBackup(BackupInfo item)
        {
            return this._Context.Backups.Where(x => x.BaseBackupID == item.ID).ToList();
        }
    }
}