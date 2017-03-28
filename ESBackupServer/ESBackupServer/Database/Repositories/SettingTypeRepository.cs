using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.Database.Repositories
{
    internal class SettingTypeRepository : AbRepository<SettingType>
    {
        #region Singleton
        private static SettingTypeRepository _Instance { get; set; }
        private SettingTypeRepository()
        {
        }
        public static SettingTypeRepository GetInstance()
        {
            if (SettingTypeRepository._Instance == null)
                SettingTypeRepository._Instance = new SettingTypeRepository();

            return SettingTypeRepository._Instance;
        }
        #endregion
        #region AbRepository
        protected override void Add(SettingType item)
        {
            this._Context.SettingsTypes.Add(item);
            this._Context.SaveChanges();
        }
        internal override SettingType Find(object id)
        {
            return this._Context.SettingsTypes.Find(id);
        }
        internal override List<SettingType> FindAll()
        {
            return this._Context.SettingsTypes.ToList();
        }
        internal override void Remove(SettingType item)
        {
            this._Context.SettingsTypes.Remove(item);
            this._Context.SaveChanges();
        }
        internal override void Update(SettingType item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}