﻿using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupTemplateSettingRepository : AbRepository<BackupTemplateSetting>
    {
        #region Singleton
        private BackupTemplateSettingRepository()
        {

        }
        private static BackupTemplateSettingRepository _Instance { get; set; }
        internal static BackupTemplateSettingRepository GetInstance()
        {
            if (BackupTemplateSettingRepository._Instance == null)
                BackupTemplateSettingRepository._Instance = new BackupTemplateSettingRepository();
            return BackupTemplateSettingRepository._Instance;
        }
        #endregion
        #region AbRepository
        protected override void Add(BackupTemplateSetting item)
        {
            this._Context.Settings.Add(item);
            this._Context.SaveChanges();
        }
        internal override BackupTemplateSetting Find(object id)
        {
            return this._Context.Settings.Find(id);
        }
        internal override List<BackupTemplateSetting> FindAll()
        {
            return this._Context.Settings.ToList();
        }
        internal override void Remove(BackupTemplateSetting item)
        {
            this._Context.Settings.Remove(item);
            this._Context.SaveChanges();
        }

        internal override void Update(BackupTemplateSetting item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}