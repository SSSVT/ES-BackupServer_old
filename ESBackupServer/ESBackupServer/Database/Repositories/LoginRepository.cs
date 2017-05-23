using ESBackupServer.App.Objects.Authentication;
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
            return this._Context.Logins.Where(x => x.IDClient == client.ID && x.UTCExpiration > DateTime.UtcNow).FirstOrDefault();
        }

        internal List<Login> FindByClient(int ID)
        {
            return this._Context.Logins.Where(x => x.IDClient == ID).ToList();
        }

        /// <summary>
        /// Create, add to database and return
        /// </summary>
        /// <param name="client">Client instance</param>
        /// <returns></returns>
        internal LoginResponse Create(Client client, IPAddress EndpointIP)
        {
            Login login = this.Find(client);
            if (login != null && login.IP == EndpointIP.ToString()) //IP adresa je ok
            {
                login.UTCExpiration = DateTime.UtcNow.AddMinutes(15); //Refresh expiration
                this.Update(login);
            }
            else
            {
                this.Add(new Login(client, DateTime.UtcNow, EndpointIP));
                login = this.Find(client);
            }

            return new LoginResponse()
            {
                SessionID = login.ID,
                UTCExpiration = login.UTCExpiration
            };
        }
        internal bool IsSessionIDValid(Login login)
        {
            if (login.UTCExpiration > DateTime.UtcNow && login.IP == this._NetInfo.GetClientIP().ToString())
            {
                login.UTCExpiration = DateTime.UtcNow.AddMinutes(15);
                return true;
            }
            return false;
        }
    }
}