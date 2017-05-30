using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Repositories
{
    internal class SmtpConfigurationRepository : AbRepository<SmtpConfiguration>
    {
        #region AbRepository
        protected override void Add(SmtpConfiguration item)
        {
            this._Context.SmtpConfiguration.Add(item);
            this.SaveChanges();
        }

        internal override SmtpConfiguration Find(object id)
        {
            return this._Context.SmtpConfiguration.Find(id);
        }

        internal override List<SmtpConfiguration> FindAll()
        {
            return this._Context.SmtpConfiguration.ToList();
        }

        internal override void Remove(SmtpConfiguration item)
        {
            this._Context.SmtpConfiguration.Remove(item);
            this.SaveChanges();
        }

        internal override void Update(SmtpConfiguration item)
        {
            SmtpConfiguration config = this.Find(item.ID);
            config.Server = item.Server;
            config.Port = item.Port;
            config.Username = item.Username;
            config.Password = item.Password;
            config.Method = item.Method;
            config.Protocol = item.Protocol;
            config.IsActive = item.IsActive;
            this.SaveChanges();
        }
        #endregion
    }
}