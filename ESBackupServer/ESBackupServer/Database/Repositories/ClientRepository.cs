using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.Database.Repositories
{
    internal class ClientRepository : AbRepository<Client>
    {
        #region Singleton
        private static ClientRepository _instance;
        private ClientRepository()
        {
        }
        public static ClientRepository GetInstance()
        {
            if (ClientRepository._instance == null)
                ClientRepository._instance = new ClientRepository();

            return ClientRepository._instance;
        }
        #endregion
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