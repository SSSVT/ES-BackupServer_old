using ESBackupServer.Database.Objects;
using System;
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
            if (!item.IsDifferential)
            {
                foreach (Backup diff in this._Context.Backups.Where(x => x.BaseFullBackupID == item.ID).ToList())
                    this._Context.Backups.Remove(diff);
            }
            this._Context.Backups.Remove(item);
            this.SaveChanges();
        }
        internal override void Update(Backup item)
        {
            Backup backup = this.Find(item.ID);
            backup.IDClient = item.IDClient;
            backup.Name = item.Name;
            backup.Description = item.Description;
            backup.Source = item.Source;
            backup.Destination = item.Destination;
            backup.IsDifferential = item.IsDifferential;
            backup.Expiration = item.Expiration;
            backup.Compressed = item.Compressed;
            backup.Start = item.Start;
            backup.End = item.End;
            backup.Status = item.Status;
            this.SaveChanges();
        }
        internal void Update(Backup item, bool overload)
        {
            Backup backup = this.Find(item.ID);
            backup.Name = item.Name;
            backup.Description = item.Description;
            backup.Expiration = item.Expiration;
            this.SaveChanges();
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
    }
}