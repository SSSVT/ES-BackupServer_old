using ESBackupServer.App.Objects;
using ESBackupServer.App.Objects.Factories.Config;
using ESBackupServer.App.Objects.Filters;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System.Collections.Generic;
using System;
using ESBackupServer.App.Interfaces.CRON;
using ESBackupServer.App.Components.CRON;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ESBackupServerAdminService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ESBackupServerAdminService.svc or ESBackupServerAdminService.svc.cs at the Solution Explorer and start debugging.
    public class ESBackupServerAdminService : IESBackupServerAdminService
    {
        public ESBackupServerAdminService()
        {
            this._scheduler = TaskScheduler.GetInstance();
        }        
        ~ESBackupServerAdminService()
        {
            this._scheduler.Stop(); 
        }

        #region Properties
        #region Repositories
        private LoginRepository _LoginRepository { get; set; } = LoginRepository.GetInstance();
        private EmailRepository _EmailRepository { get; set; } = EmailRepository.GetInstance();
        private ClientRepository _ClientRepository { get; set; } = ClientRepository.GetInstance();
        private LogRepository _LogRepository { get; set; } = LogRepository.GetInstance();
        private BackupRepository _BackupRepository { get; set; } = BackupRepository.GetInstance();
        private BackupTemplateRepository _BackupTemplateRepository { get; set; } = BackupTemplateRepository.GetInstance();
        private BackupTemplatePathRepository _BackupTemplatePathRepository { get; set; } = BackupTemplatePathRepository.GetInstance();
        private AdministratorRepository _AdministratorRepository { get; set; } = AdministratorRepository.GetInstance();
        #endregion
        private ITaskScheduler _scheduler { get; set; }
        private ConfigurationFactory _ConfigFactory { get; set; } = new ConfigurationFactory();
        #endregion

        #region Get
        public List<BackupInfo> GetBackupsByClientID(int id)
        {
            return this._BackupRepository.FindByClientID(id);
        }
        public BackupInfo GetBackupByID(long id)
        {
            return this._BackupRepository.Find(id);
        }
        public List<Client> GetClients(Filter filter, Sort sort)
        {
            return this._ClientRepository.Find(filter, sort);
        }
        public Client GetClientByID(int ID)
        {
            return this._ClientRepository.Find(ID);
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
        public Administrator GetProfile(string username)
        {
            Administrator admin = this._AdministratorRepository.FindByUsername(username);
            admin.Emails = this._EmailRepository.Find(admin);
            return admin;
        }

        public List<Login> GetLoginsByClient(int ID)
        {
            return this._LoginRepository.FindByClient(ID);
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
        public void UpdateBackup(BackupInfo item)
        {
            this._BackupRepository.Update(item);
        }

        public void SaveTemplate(BackupTemplate item)
        {
            this._BackupTemplateRepository.Update(item);
            this.ClientConfigUpdated(item.IDClient);
        }
        public void SetTemplateStatus(long id, bool IsEnabled)
        {
            BackupTemplate item = this._BackupTemplateRepository.Find(id);
            item.Enabled = IsEnabled;
            this._BackupTemplateRepository.Update(item);
            this.ClientConfigUpdated(item.IDClient);
        }
        public void RemoveBackupTemplate(long id)
        {
            BackupTemplate item = this._BackupTemplateRepository.Find(id);
            this._BackupTemplateRepository.Remove(item);
            this.ClientConfigUpdated(item.IDClient);
        }
        public void RemoveBackupTemplatePath(Guid id)
        {
            BackupTemplatePath item = this._BackupTemplatePathRepository.Find(id);
            this._BackupTemplatePathRepository.Remove(item);
            this.ClientConfigUpdated(this._BackupTemplateRepository.Find(item.IDBackupTemplate).IDClient);
        }

        public void UpdateAdministrator(Administrator admin)
        {
            this._AdministratorRepository.Update(admin);
        }
        public void ClientConfigUpdated(int id)
        {
            Client c = this._ClientRepository.Find(id);
            c.UTCLastConfigUpdate = DateTime.UtcNow;
            this._ClientRepository.Update(c);
        }
        #endregion
    }
}