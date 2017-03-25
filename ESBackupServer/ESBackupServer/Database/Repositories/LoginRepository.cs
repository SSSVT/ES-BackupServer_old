using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.Database.Repositories
{
    internal class LoginRepository : AbRepository<Login>
    {
        #region Singleton
        private static LoginRepository _instance;
        private LoginRepository()
        {
        }
        public static LoginRepository GetInstance()
        {
            if (LoginRepository._instance == null)
                LoginRepository._instance = new LoginRepository();

            return LoginRepository._instance;
        }
        #endregion
        #region AbRepository
        internal override void Add(Login item)
        {
            this._Context.Logins.Add(item);
        }
        internal override Login FindByID(int id)
        {
            return this._Context.Logins.Find(id);
        }
        internal override void Remove(Login item)
        {
            this._Context.Logins.Remove(item);
        }
        internal override void Update(Login item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}