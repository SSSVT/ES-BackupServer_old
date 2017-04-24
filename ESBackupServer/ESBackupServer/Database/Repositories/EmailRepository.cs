using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Repositories
{
    internal class EmailRepository : AbRepository<Email>
    {
        //TODO: Implement
        #region AbRepository
        protected override void Add(Email item)
        {
            throw new NotImplementedException();
        }
        internal override Email Find(object id)
        {
            throw new NotImplementedException();
        }
        internal override List<Email> FindAll()
        {
            throw new NotImplementedException();
        }
        internal override void Remove(Email item)
        {
            throw new NotImplementedException();
        }
        internal override void Update(Email item)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}