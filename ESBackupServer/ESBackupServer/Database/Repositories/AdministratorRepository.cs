using ESBackupServer.Database.Objects;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class AdministratorRepository : AbRepository<Administrator>
    {
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
            //admin.Username = item.Username;
            admin.Password = item.Password;
            this.SaveChanges();
        }
        #endregion
    }
}