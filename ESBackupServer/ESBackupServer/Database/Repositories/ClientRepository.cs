using ESBackupServer.App.Objects.Filters;
using ESBackupServer.Database.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            this.SaveChanges();
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
            this.SaveChanges();
        }
        internal override void Update(Client item)
        {
            Client client = this.Find(item.ID);
            client.Name = item.Name;
            client.Description = item.Description;
            client.Username = item.Username;
            client.LastBackupTime = item.LastBackupTime;
            client.Status = item.Status;
            client.Hardware_ID = item.Hardware_ID;
            client.Password = item.Password;
            client.Salt = item.Salt;
            this.SaveChanges();
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
        internal List<Client> Find(Filter filter, Sort sort)
        {
            switch (filter)
            {
                case Filter.Verified:
                    return (sort == Sort.Asc)
                        ? this._Context.Clients.Where(x => x.Status == 0).OrderBy(x => x.LastReportTime).ToList()
                        : this._Context.Clients.Where(x => x.Status == 0).OrderByDescending(x => x.LastReportTime).ToList();
                case Filter.Unverified:
                    return (sort == Sort.Asc)
                        ? this._Context.Clients.Where(x => x.Status == 1).OrderBy(x => x.LastReportTime).ToList()
                        : this._Context.Clients.Where(x => x.Status == 1).OrderByDescending(x => x.LastReportTime).ToList();
                case Filter.Banned:
                    return (sort == Sort.Asc)
                        ? this._Context.Clients.Where(x => x.Status == 2).OrderBy(x => x.LastReportTime).ToList()
                        : this._Context.Clients.Where(x => x.Status == 2).OrderByDescending(x => x.LastReportTime).ToList();
                default:
                    return (sort == Sort.Asc)
                        ? this._Context.Clients.OrderBy(x => x.LastReportTime).ToList()
                        : this._Context.Clients.OrderByDescending(x => x.LastReportTime).ToList();
            }
        }

        internal Client Find(string name, string hwid)
        {
            return this._Context.Clients.Where(x => x.Name == name && x.Hardware_ID == hwid).FirstOrDefault();
        }

        internal Client CreateClient(string name, string hwid)
        {
            this.Add(new Client()
            {
                Name = name,
                Hardware_ID = hwid
            });
            return this.Find(name, hwid);
        }

        internal void SaveEmails(int clientID, List<ClientEmail> list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count - 1; i++)
            {
                sb.Append(list[i].Value);

                if (i != list.Count - 1)
                    sb.Append(";");
            }

            this._Context.Clients.Where(x => x.ID == clientID).FirstOrDefault().Emails = sb.ToString();
            this.SaveChanges();
        }
    }
}