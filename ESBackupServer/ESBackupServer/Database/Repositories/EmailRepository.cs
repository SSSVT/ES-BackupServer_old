using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Repositories
{
    internal class EmailRepository : AbRepository<Email>
    {
        #region AbRepository
        protected override void Add(Email item)
        {
            this._Context.Emails.Add(item);
        }
        internal override Email Find(object id)
        {
            return this._Context.Emails.Find(id);
        }
        internal override List<Email> FindAll()
        {
            return this._Context.Emails.ToList();
        }
        internal override void Remove(Email item)
        {
            this._Context.Emails.Remove(item);
        }
        internal override void Update(Email item)
        {
            Email email = this.Find(item.ID);
            email.Address = item.Address;
            email.IsDefault = item.IsDefault;
            this.SaveChanges();
        }
        #endregion

        internal List<Email> Find(Administrator admin)
        {
            return this._Context.Emails.Where(x => x.IDAdministrator == admin.ID).ToList();
        }
    }
}