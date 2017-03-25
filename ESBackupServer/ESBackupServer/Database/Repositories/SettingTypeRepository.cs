﻿using ESBackupServer.Database.Objects;
using System;

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
        internal override void Add(SettingType item)
        {
            this._Context.SettingsTypes.Add(item);
        }
        internal override SettingType FindByID(int id)
        {
            return this._Context.SettingsTypes.Find(id);
        }
        internal override void Remove(SettingType item)
        {
            this._Context.SettingsTypes.Remove(item);
        }
        internal override void Update(SettingType item)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}