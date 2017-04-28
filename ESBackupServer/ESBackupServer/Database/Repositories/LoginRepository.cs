using ESBackupServer.App.Objects.Components.Net;
using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ESBackupServer.Database.Repositories
{
    internal class LoginRepository : AbRepository<Login>
    {
        #region Singleton
        private LoginRepository()
        {

        }
        private static LoginRepository _Instance { get; set; }
        internal static LoginRepository GetInstance()
        {
            if (LoginRepository._Instance == null)
                LoginRepository._Instance = new LoginRepository();
            return LoginRepository._Instance;
        }
        #endregion
        #region Properties
        private NetInfoObtainer _NetInfo { get; set; } = new NetInfoObtainer();
        #endregion
        #region AbRepository
        protected override void Add(Login item)
        {
            this._Context.Logins.Add(item);
            this.SaveChanges();
        }
        internal override Login Find(object id)
        {
            return this._Context.Logins.Find(id);
        }
        internal override List<Login> FindAll()
        {
            return this._Context.Logins.ToList();
        }
        internal override void Remove(Login item)
        {
            this._Context.Logins.Remove(item);
            this.SaveChanges();
        }
        internal override void Update(Login item)
        {
            Login login = this.Find(item.ID);
            login.IDClient = item.IDClient;
            login.UTCTime = item.UTCTime;
            login.UTCExpiration = item.UTCExpiration;
            login.IP = item.IP;
            this.SaveChanges();
        }
        #endregion

        internal Login Find(Client client)
        {
            return this._Context.Logins.Where(x => x.IDClient == client.ID && x.UTCExpiration < DateTime.UtcNow).FirstOrDefault();
        }

        /// <summary>
        /// Create, add to database and return
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <returns></returns>
        internal Login Create(Client client, IPAddress EndpointIP)
        {
            Login login = this.Find(client);

            if (login != null && new IPAddress(login.IP) == EndpointIP) //IP adresa je ok
            {
                login.UTCExpiration = DateTime.UtcNow.AddMinutes(15); //Refresh expiration
            }
            else if (login != null && new IPAddress(login.IP) != EndpointIP)
            {
                login.UTCExpiration = DateTime.UtcNow;
            }
            else
            {
                this.Add(new Login(client, DateTime.UtcNow, EndpointIP));
                return this.Find(client);
            }
            this.Update(login);
            return login;
        }
        internal bool IsSessionIDValid(Login login)
        {
            if (login.UTCExpiration < DateTime.UtcNow && new IPAddress(login.IP) == this._NetInfo.GetClientIP())
            {
                login.UTCExpiration = DateTime.UtcNow.AddMinutes(15);
                return true;
            }
            return false;
        }
    }
}