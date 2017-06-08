using ESBackupServer.Database.Objects;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupRepository : AbRepository<BackupInfo>
    {
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
                item.EmailSent = !new BackupTemplateRepository().Find(item.IDBackupTemplate).IsEmailNotificationEnabled;
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
        #region Local properties
        private ClientRepository _ClientRepository = new ClientRepository();
        #endregion
        internal List<BackupInfo> FindByClientID(int ID)
        {
            return this._Context.Backups.Where(x => x.IDClient == ID).ToList();
        }
        internal void Remove(long id)
        {
            this.Remove(this.Find(id));
        }
        internal void Remove(Client item)
        {
            foreach (BackupInfo info in this.FindByClientID(item.ID))
            {
                this.Remove(info);
            }
        }
        private List<BackupInfo> FindByBaseBackup(BackupInfo item)
        {
            return this._Context.Backups.Where(x => x.BaseBackupID == item.ID).ToList();
        }
        internal List<BackupInfo> FindBackupsWithUnsentEmailByAdmin(long IDAdmin)
        {
            List<Client> clientList = this._ClientRepository.FindByAdmin(IDAdmin);
            if (clientList.Count > 1)
            {
                List<BackupInfo> backupList = new List<BackupInfo>();
                foreach (Client client in clientList)
                {
                    foreach (BackupInfo backup in client.Backups)
                    {
                        backupList.Add(backup);
                    }
                }
                return backupList;
            }
            else
                return this._Context.Backups.Where(x => x.EmailSent == false && x.IDClient == clientList[0].ID).ToList();
        }

        internal BackupInfo GetLastTemplateBackup(long id)
        {
            return this._Context.Backups.Where(x => x.IDBackupTemplate == id).OrderBy(x => x.UTCEnd).LastOrDefault();
        }

        internal List<BackupInfo> GetPreviousBackups(long id)
        {
            List<BackupInfo> list = new List<BackupInfo>();
            BackupInfo backup = this.Find(id);

            while (backup.BackupType != 0)
            {
                list.Add(backup);
                backup = this.Find(backup.BaseBackupID);
            }

            return list;
        }

        internal long GetCount(int clientID, byte code)
        {
            return this._Context.Backups.Where(x => x.IDClient == clientID && x.Status == code && x.EmailSent == false).LongCount();
        }
    }
}