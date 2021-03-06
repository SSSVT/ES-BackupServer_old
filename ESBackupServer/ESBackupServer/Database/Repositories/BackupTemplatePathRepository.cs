﻿using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class BackupTemplatePathRepository : AbRepository<BackupTemplatePath>
    {
        #region AbRepository
        protected override void Add(BackupTemplatePath item)
        {
            this._Context.TemplatesPaths.Add(item);
            this.SaveChanges();
        }
        internal override BackupTemplatePath Find(object id)
        {
            return this._Context.TemplatesPaths.Find(id);
        }
        internal override List<BackupTemplatePath> FindAll()
        {
            return this._Context.TemplatesPaths.ToList();
        }
        internal override void Remove(BackupTemplatePath item)
        {
            this._Context.TemplatesPaths.Remove(item);
            this.SaveChanges();
        }
        internal override void Update(BackupTemplatePath item)
        {
            BackupTemplatePath path = this.Find(item.ID);
            if (path == null)
            {
                this.Add(item);
            }
            else
            {
                path.IDBackupTemplate = item.IDBackupTemplate;
                path.PathOrder = item.PathOrder;
                path.TargetType = item.TargetType;
                path.Source = item.Source;
                path.Destination = item.Destination;

                path.Username = item.Username;
                path.Password = item.Password;
                this.SaveChanges();
            }            
        }
        #endregion

        internal List<BackupTemplatePath> Find(BackupTemplate template)
        {
            return this._Context.TemplatesPaths.Where(x => x.IDBackupTemplate == template.ID).ToList();
        }

        internal void Remove(Guid id)
        {
            this.Remove(this.Find(id));
        }
    }
}