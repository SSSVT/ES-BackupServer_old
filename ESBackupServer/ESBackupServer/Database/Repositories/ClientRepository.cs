using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class ClientRepository : AbRepository<Client>
    {
        #region Singleton
        private ClientRepository()
        {

        }
        private static ClientRepository _Instance { get; set; }
        internal static ClientRepository GetInstance()
        {
            if (ClientRepository._Instance == null)
                ClientRepository._Instance = new ClientRepository();
            return ClientRepository._Instance;
        }
        #endregion
        #region AbRepository
        protected override void Add(Client item)
        {
            this._Context.Clients.Add(item);
            this._Context.SaveChanges();
        }
        internal override Client Find(object id)
        {
            return this._Context.Clients.Find(id);
        }
        internal override List<Client> FindAll()
        {
            return this._Context.Clients.ToList();
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

        internal bool IsLoginValid(Client client, string password)
        {
            //TODO: Osolit heslo
            return (client.Password == password) ? true : false;
        }
        internal Client FindByUsername(string username)
        {
            return this._Context.Clients.Where(x => x.Username == username).FirstOrDefault();
        }

        
    }
}