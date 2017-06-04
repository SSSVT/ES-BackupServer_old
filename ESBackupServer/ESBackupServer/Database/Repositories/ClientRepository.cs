using ESBackupServer.App.Objects.Filters;
using ESBackupServer.Database.Objects;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class ClientRepository : AbRepository<Client>
    {
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
            new BackupRepository().Remove(item);
            new LogRepository().Remove(item);
            this._Context.Clients.Remove(item);
            this.SaveChanges();
        }
        internal override void Update(Client item)
        {
            Client client = this.Find(item.ID);
            //client.IDAdministrator = item.IDAdministrator;
            //client.Name = item.Name;
            client.Description = item.Description;
            //client.HardwareID = item.HardwareID;
            //client.Username = item.Username;
            //client.Password = item.Password;
            client.Status = item.Status;
            client.StatusReportEnabled = item.StatusReportEnabled;
            client.ReportInterval = item.ReportInterval;
            client.UTCLastStatusReportTime = item.UTCLastStatusReportTime;
            client.UTCLastBackupTime = item.UTCLastBackupTime;
            client.UTCLastConfigUpdate = item.UTCLastConfigUpdate;
            this.SaveChanges();
        }
        #endregion

        internal bool IsLoginValid(Client client, string password)
        {
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
                        ? this._Context.Clients.Where(x => x.Status == 0).OrderBy(x => x.UTCLastStatusReportTime).ToList()
                        : this._Context.Clients.Where(x => x.Status == 0).OrderByDescending(x => x.UTCLastStatusReportTime).ToList();
                case Filter.Unverified:
                    return (sort == Sort.Asc)
                        ? this._Context.Clients.Where(x => x.Status == 1).OrderBy(x => x.UTCLastStatusReportTime).ToList()
                        : this._Context.Clients.Where(x => x.Status == 1).OrderByDescending(x => x.UTCLastStatusReportTime).ToList();
                case Filter.Banned:
                    return (sort == Sort.Asc)
                        ? this._Context.Clients.Where(x => x.Status == 2).OrderBy(x => x.UTCLastStatusReportTime).ToList()
                        : this._Context.Clients.Where(x => x.Status == 2).OrderByDescending(x => x.UTCLastStatusReportTime).ToList();
                default:
                    return (sort == Sort.Asc)
                        ? this._Context.Clients.OrderBy(x => x.UTCLastStatusReportTime).ToList()
                        : this._Context.Clients.OrderByDescending(x => x.UTCLastStatusReportTime).ToList();
            }
        }

        internal Client Find(string name, string hwid)
        {
            return this._Context.Clients.Where(x => x.Name == name && x.HardwareID == hwid).FirstOrDefault();
        }
        internal Client CreateClient(Administrator admin, string name, string hwid)
        {
            this.Add(new Client()
            {
                IDAdministrator = admin.ID,
                Name = name,
                HardwareID = hwid
            });
            return this.Find(name, hwid);
        }
        internal List<Client> FindByAdmin(long IDAdmin)
        {
            return this._Context.Clients.Where(x => x.IDAdministrator == IDAdmin).ToList();
        }
    }
}