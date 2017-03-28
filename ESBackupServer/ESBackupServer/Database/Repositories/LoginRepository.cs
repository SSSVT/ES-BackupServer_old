using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class LoginRepository : AbRepository<Login>
    {
        #region Singleton
        private static LoginRepository _Instance { get; set; }
        private LoginRepository()
        {
        }
        public static LoginRepository GetInstance()
        {
            if (LoginRepository._Instance == null)
                LoginRepository._Instance = new LoginRepository();

            return LoginRepository._Instance;
        }
        #endregion
        #region AbRepository
        protected override void Add(Login item)
        {
            this._Context.Logins.Add(item);
            this._Context.SaveChanges();
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
            this._Context.SaveChanges();
        }
        internal override void Update(Login item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion

        internal Login Find(Client client, DateTime time, bool active)
        {
            return this._Context.Logins.Where(x => x.IDClient == client.ID && x.UTCTime == time && x.Active == active).FirstOrDefault();
        }

        internal Login Create(Client client)
        {
            LoginRepository repo = LoginRepository.GetInstance();
            DateTime time = DateTime.UtcNow;
            repo.Add(new Login(client, time));
            return repo.Find(client, time, true);
        }
    }
}