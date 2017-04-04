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
        //TODO: Check
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
            backup.Type = item.Type;
            backup.Expiration = item.Expiration;
            backup.Compressed = item.Compressed;
            backup.Start = item.Start;
            backup.End = item.End;
            backup.Status = item.Status;
            this.SaveChanges();
        }
        #endregion
        //TODO: Check
        internal List<Backup> Find(Client client)
        {
            return this._Context.Backups.Where(x => x.IDClient == client.ID).ToList();
        }
    }
}