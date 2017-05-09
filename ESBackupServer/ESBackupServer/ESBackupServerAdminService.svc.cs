using ESBackupServer.App.Objects;
using ESBackupServer.App.Objects.Factories.Config;
using ESBackupServer.App.Objects.Filters;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System.Collections.Generic;
using System;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ESBackupServerAdminService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ESBackupServerAdminService.svc or ESBackupServerAdminService.svc.cs at the Solution Explorer and start debugging.
    public class ESBackupServerAdminService : IESBackupServerAdminService
    {
        public ESBackupServerAdminService()
        {
            //onStart
        }
        ~ESBackupServerAdminService()
        {
            //onStop
        }

        #region Properties
        #region Repositories
        private AdministratorRepository _AdminRepository { get; set; } = AdministratorRepository.GetInstance();
        private LoginRepository _LoginRepository { get; set; } = LoginRepository.GetInstance();
        private ClientRepository _ClientRepository { get; set; } = ClientRepository.GetInstance();
        private LogRepository _LogRepository { get; set; } = LogRepository.GetInstance();
        private BackupRepository _BackupRepository { get; set; } = BackupRepository.GetInstance();
        private BackupTemplateRepository _BackupTemplateRepository { get; set; } = BackupTemplateRepository.GetInstance();
        private BackupTemplatePathRepository _BackupTemplatePathRepository { get; set; } = BackupTemplatePathRepository.GetInstance();
        private AdministratorRepository _AdministratorRepository { get; set; } = AdministratorRepository.GetInstance();
        #endregion

        private ConfigurationFactory _ConfigFactory { get; set; } = new ConfigurationFactory();
        #endregion

        #region Get
        public List<Backup> GetBackupsByClientID(int id)
        {
            return this._BackupRepository.FindByClientID(id);
        }
        public Backup GetBackupByID(long id)
        {
            return this._BackupRepository.Find(id);
        }
        public List<Client> GetClients(Filter filter, Sort sort)
        {
            return this._ClientRepository.Find(filter, sort);
        }
        public Configuration GetConfigurationByClientID(int id)
        {
            return this._ConfigFactory.Create(this._ClientRepository.Find(id));
        }        
        public List<Log> GetLogsByClientID(int id)
        {
            return this._LogRepository.FindByClientID(id);
        }        
        public List<Log> GetLogsByBackupID(long id)
        {
            return this._LogRepository.FindByBackupID(id);
        }
        public BackupTemplate GetTemplateByID(int id)
        {
            return this._BackupTemplateRepository.Find(id);
        }

        public bool Login(string username, string password)
        {
            Administrator admin = this._AdministratorRepository.FindByUsername(username);

            if (admin == null)
                return false;
            
            return this._AdministratorRepository.IsLoginValid(admin,password);   
        }
        public Administrator GetProfile(Guid sessionID)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Set
        public void UpdateClient(Client item)
        {
            this._ClientRepository.Update(item);
        }
        public void RemoveBackup(long id)
        {
            this._BackupRepository.Remove(id);
        }
        public void UpdateBackup(Backup item)
        {
            this._BackupRepository.Update(item);
        }

        public void SaveTemplate(BackupTemplate item)
        {
            this._BackupTemplateRepository.Update(item);
        }
        public void SetTemplateStatus(long id, bool IsEnabled)
        {
            BackupTemplate item = this._BackupTemplateRepository.Find(id);
            item.Enabled = IsEnabled;
            this._BackupTemplateRepository.Update(item);
        }
        public void RemoveBackupTemplate(long id)
        {
            this._BackupTemplateRepository.Remove(id);
        }

        public void RemoveBackupTemplatePath(Guid id)
        {
            this._BackupTemplatePathRepository.Remove(id);
        }
        #endregion
    }
}