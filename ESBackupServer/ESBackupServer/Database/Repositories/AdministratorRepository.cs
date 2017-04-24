using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Repositories
{
    internal class AdministratorRepository : AbRepository<Administrator>
    {
        //TODO: Implement
        #region AbRepository
        protected override void Add(Administrator item)
        {
            throw new NotImplementedException();
        }
        internal override Administrator Find(object id)
        {
            throw new NotImplementedException();
        }
        internal override List<Administrator> FindAll()
        {
            throw new NotImplementedException();
        }
        internal override void Remove(Administrator item)
        {
            throw new NotImplementedException();
        }
        internal override void Update(Administrator item)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}