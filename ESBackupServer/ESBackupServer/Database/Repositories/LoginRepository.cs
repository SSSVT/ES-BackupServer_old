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
        internal Login Create(Client client)
        {
            Login login = this.Find(client);

            if (login != null && new IPAddress(login.IP) == this._NetInfo.GetClientIP())
                return login;
            else if (login != null && new IPAddress(login.IP) != this._NetInfo.GetClientIP())
                login.UTCExpiration = DateTime.UtcNow;

            LoginRepository repo = LoginRepository.GetInstance();
            DateTime time = DateTime.UtcNow;
            repo.Add(new Login(client, time));
            return repo.Find(client);
        }
        internal bool IsSessionIDValid(Login login)
        {
            return login.UTCExpiration < DateTime.UtcNow;
        }
    }
}