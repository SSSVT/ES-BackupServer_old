using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.Database.Repositories
{
    internal class SettingRepository : AbRepository<Setting>
    {
        #region Singleton
        private static SettingRepository _Instance { get; set; }
        private SettingRepository()
        {
        }
        public static SettingRepository GetInstance()
        {
            if (SettingRepository._Instance == null)
                SettingRepository._Instance = new SettingRepository();

            return SettingRepository._Instance;
        }
        #endregion
        #region AbRepository
        protected override void Add(Setting item)
        {
            this._Context.Settings.Add(item);
            this._Context.SaveChanges();
        }
        internal override Setting Find(object id)
        {
            return this._Context.Settings.Find(id);
        }
        internal override void Remove(Setting item)
        {
            this._Context.Settings.Remove(item);
            this._Context.SaveChanges();
        }
        internal override void Update(Setting item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}