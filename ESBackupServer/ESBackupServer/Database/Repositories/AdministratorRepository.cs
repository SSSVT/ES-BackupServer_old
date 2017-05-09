using ESBackupServer.Database.Objects;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class AdministratorRepository : AbRepository<Administrator>
    {
        private EmailRepository _EmailRepository { get; set; } = EmailRepository.GetInstance;

        #region Singleton
        private AdministratorRepository()
        {

        }
        private static AdministratorRepository _Instance { get; set; }
        internal static AdministratorRepository GetInstance()
        {
            if (AdministratorRepository._Instance == null)
                AdministratorRepository._Instance = new AdministratorRepository();
            return AdministratorRepository._Instance;
        }
        #endregion
        #region AbRepository
        protected override void Add(Administrator item)
        {
            this._Context.Administrators.Add(item);
        }
        internal override Administrator Find(object id)
        {
            return this._Context.Administrators.Find(id);
        }
        internal override List<Administrator> FindAll()
        {
            return this._Context.Administrators.ToList();
        }
        internal override void Remove(Administrator item)
        {
            this._Context.Administrators.Remove(item);
        }
        internal override void Update(Administrator item)
        {
            Administrator admin = this.Find(item.ID);
            admin.FirstName = item.FirstName;
            admin.LastName = item.LastName;
            admin.Password = item.Password;

            foreach (Email email in item.Emails)
            {
                this._EmailRepository.Update(email);
            }

            this.SaveChanges();
        }
        #endregion
        internal Administrator FindByUsername(string username)
        {
            return this._Context.Administrators.Where(x => x.Username == username).FirstOrDefault();
        }
        internal bool IsLoginValid(Administrator admin, string password)
        {
            //TODO: Salt password
            return (admin.Password == password) ? true : false;
        }
    }
}