using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.Database.Repositories
{
    internal class ClientRepository : AbRepository<Client>
    {
        #region Singleton
        private static ClientRepository _Instance { get; set; }
        private ClientRepository()
        {
        }
        public static ClientRepository GetInstance()
        {
            if (ClientRepository._Instance == null)
                ClientRepository._Instance = new ClientRepository();

            return ClientRepository._Instance;
        }
        #endregion
        #region AbRepository
        internal override void Add(Client item)
        {
            this._Context.Clients.Add(item);
            this._Context.SaveChanges();
        }
        internal override Client FindByID(int id)
        {
            return this._Context.Clients.Find(id);
        }
        internal override void Remove(Client item)
        {
            this._Context.Clients.Remove(item);
            this._Context.SaveChanges();
        }
        internal override void Update(Client item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion

        internal bool IsLoginValid(string username, string password)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}