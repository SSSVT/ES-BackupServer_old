using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.Database.Repositories
{
    internal class ClientRepository : AbRepository<Client>
    {
        #region AbRepository
        internal override void Add(Client item)
        {
            this._Context.Clients.Add(item);
        }
        internal override Client FindByID(int id)
        {
            return this._Context.Clients.Find(id);
        }
        internal override void Remove(Client item)
        {
            this._Context.Clients.Remove(item);
        }
        internal override void Update(Client item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}