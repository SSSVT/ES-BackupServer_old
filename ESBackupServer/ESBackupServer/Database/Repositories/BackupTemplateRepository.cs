﻿using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupTemplateRepository : AbRepository<BackupTemplate>
    {
        private BackupTemplatePathRepository _BackupTemplatePathRepository { get; set; } = BackupTemplatePathRepository.GetInstance();

        #region Singleton
        private BackupTemplateRepository()
        {

        }
        private static BackupTemplateRepository _Instance { get; set; }
        internal static BackupTemplateRepository GetInstance()
        {
            if (BackupTemplateRepository._Instance == null)
                BackupTemplateRepository._Instance = new BackupTemplateRepository();
            return BackupTemplateRepository._Instance;
        }
        #endregion
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
            BackupTemplate template = this.Find(item.ID);
            if (template == null)
            {
                this.Add(item); //nenalezeno - nové

                byte[] arr = new byte[16];
                new Random().NextBytes(arr);
                Guid tmpId = new Guid(arr);               
                BackupTemplate tmp = this.Find(tmpId);

                foreach (BackupTemplatePath path in item.Paths)
                {
                    path.IDBackupTemplate = tmp.ID;
                }
            }
            else
            {
                template.IDClient = item.IDClient;
                template.Name = item.Name;
                template.Description = item.Description;
                template.BackupType = item.BackupType;
                template.DaysToExpiration = item.DaysToExpiration;
                template.Compression = item.Compression;
                template.SearchPattern = item.SearchPattern;
                template.BackupEmptyDirectories = item.BackupEmptyDirectories;
                template.Enabled = item.Enabled;
                template.IsNotificationEnabled = item.IsNotificationEnabled;
                template.IsEmailNotificationEnabled = item.IsEmailNotificationEnabled;
                template.CRONRepeatInterval = item.CRONRepeatInterval;
                this.SaveChanges();

                foreach (BackupTemplatePath path in item.Paths)
                {
                    this._BackupTemplatePathRepository.Update(path);
                }
            }
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
    }
}