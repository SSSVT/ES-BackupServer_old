using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.Database.Repositories
{
    internal class SettingRepository : AbRepository<Setting>
    {
        #region Singleton
        private static SettingRepository _instance;
        private SettingRepository()
        {
        }
        public static SettingRepository GetInstance()
        {
            if (SettingRepository._instance == null)
                SettingRepository._instance = new SettingRepository();

            return SettingRepository._instance;
        }
        #endregion
        #region AbRepository
        internal override void Add(Setting item)
        {
            this._Context.Settings.Add(item);
        }
        internal override Setting FindByID(int id)
        {
            return this._Context.Settings.Find(id);
        }
        internal override void Remove(Setting item)
        {
            this._Context.Settings.Remove(item);
        }
        internal override void Update(Setting item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}