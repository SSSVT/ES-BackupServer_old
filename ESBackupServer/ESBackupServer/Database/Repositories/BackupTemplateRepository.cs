using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupTemplateRepository : AbRepository<BackupTemplate>
    {
        protected BackupTemplatePathRepository _BackupTemplatePathRepository { get; set; } = new BackupTemplatePathRepository();

        #region AbRepository
        protected override void Add(BackupTemplate item)
        {
            this._Context.Templates.Add(item);
            this.SaveChanges();
        }
        internal override BackupTemplate Find(object id)
        {
            return this._Context.Templates.Find(id);
        }
        internal override List<BackupTemplate> FindAll()
        {
            return this._Context.Templates.ToList();
        }
        internal override void Remove(BackupTemplate item)
        {
            this._Context.Templates.Remove(item);
            this.SaveChanges();
        }
        internal override void Update(BackupTemplate item)
        {
            byte[] arr = new byte[16];
            new Random().NextBytes(arr);
            Guid tmpId = new Guid(arr);

            BackupTemplate template = this.Find(item.ID);
            if (template == null)
            {
                item.TmpID = tmpId;
                this.Add(item); //this.SaveChanges();
            }
            else
            {
                template.TmpID = tmpId;

                template.IDClient = item.IDClient;
                template.Name = item.Name;
                template.Description = item.Description;
                template.BackupType = item.BackupType;
                template.DaysToExpiration = item.DaysToExpiration;
                template.Compression = item.Compression;
                template.SearchPattern = item.SearchPattern;                
                template.Enabled = item.Enabled;
                template.IsNotificationEnabled = item.IsNotificationEnabled;
                template.IsEmailNotificationEnabled = item.IsEmailNotificationEnabled;
                template.CRONRepeatInterval = item.CRONRepeatInterval;
                this.SaveChanges();
            }

            BackupTemplate tmp = this.Find(tmpId);
            foreach (BackupTemplatePath path in item.Paths)
            {
                path.IDBackupTemplate = tmp.ID;
                this._BackupTemplatePathRepository.Update(path);
            }
            tmp.TmpID = null;
            this.SaveChanges();
        }
        #endregion

        internal List<BackupTemplate> Find(Client client)
        {
            return this._Context.Templates.Where(x => x.IDClient == client.ID).ToList();
        }
        internal BackupTemplate Find(Guid tmpID)
        {
            return this._Context.Templates.Where(x => x.TmpID == tmpID).FirstOrDefault();
        }
        internal void Remove(long id)
        {
            this.Remove(this.Find(id));
        }
    }
}