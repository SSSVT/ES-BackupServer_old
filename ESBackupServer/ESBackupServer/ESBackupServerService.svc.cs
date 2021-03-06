﻿using ESBackupServer.App.Components.CRON;
using ESBackupServer.App.Interfaces.CRON;
using ESBackupServer.App.Objects;
using ESBackupServer.App.Objects.Authentication;
using ESBackupServer.App.Objects.Components.Net;
using ESBackupServer.App.Objects.Factories.Config;
using ESBackupServer.App.Objects.Factories.Registration;
using ESBackupServer.App.Objects.Registration;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;
using System.Collections.Generic;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ESBackupServerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ESBackupServerService.svc or ESBackupServerService.svc.cs at the Solution Explorer and start debugging.
    public class ESBackupServerService : IESBackupServerService
    {
        public ESBackupServerService()
        {
            this._scheduler = TaskScheduler.GetInstance();
        }
        #region Properties
        private ITaskScheduler _scheduler { get; set; }
        #region Repos
        private ClientRepository _ClientRepo { get; set; } = new ClientRepository();
        private LoginRepository _LoginRepo { get; set; } = new LoginRepository();
        private LogRepository _LogRepo { get; set; } = new LogRepository();
        private BackupRepository _BackupRepository { get; set; } = new BackupRepository();
        #endregion
        #region Factories
        private ConfigurationFactory _ConfigFactory { get; set; } = new ConfigurationFactory();
        #endregion
        #region Components
        private NetInfoObtainer _NetInfo { get; set; } = new NetInfoObtainer();
        #endregion
        #endregion

        #region Registration
        public RegistrationResponse RequestRegistration(string name, string hwid)
        {
            Client item = this._ClientRepo.Find(name, hwid);
            if (item == null)
                item = this._ClientRepo.CreateClient(new AdministratorRepository().Find(1), name, hwid); //TODO: Remove debugging code
            return new UserDefinitionFactory().Create(item);
        }
        #endregion

        #region User authentication
        public LoginResponse Login(string username, string password)
        {
            Client client = this._ClientRepo.FindByUsername(username);

            if (this._ClientRepo.IsLoginValid(client, password) && client.Status == 0)
            {
                LoginResponse response = this._LoginRepo.Create(client, this._NetInfo.GetClientIP());
                this._LogRepo.Create(client, $"Session start: ID={ response.SessionID };IP={ new NetInfoObtainer().GetClientIP().ToString() };UTCTime={ DateTime.UtcNow }", LogTypeNames.Message);
                return response;
            }
            else
            {
                this._LogRepo.Create(client, $"Invalid login: IP={ new NetInfoObtainer().GetClientIP().ToString() };UTCTime={ DateTime.UtcNow }", LogTypeNames.Warning);
                return null;
            }
        }
        public bool Logout(Guid sessionID)
        {
            try
            {
                Login login = this._LoginRepo.Find(sessionID);
                Client client = this._ClientRepo.Find(login.IDClient);

                login.UTCExpiration = DateTime.UtcNow;
                this._LoginRepo.Update(login);
                this._LogRepo.Create(client, $"Session end: ID={ sessionID };UTCTime={ DateTime.UtcNow }", LogTypeNames.Message);
                return true;
            }
            catch (Exception ex)
            {
                //TODO: Save exception
                return false;
            }
        }
        #endregion

        #region Backup
        public Configuration GetConfiguration(Guid sessionID)
        {
            Login login = this._LoginRepo.Find(sessionID);
            Client client = this._ClientRepo.Find(login.IDClient);
            return (this._LoginRepo.IsSessionIDValid(login)) ? this._ConfigFactory.Create(client) : null;  
        }

        public void CreateBackup(BackupInfo backup, Guid sessionID)
        {
            Client client = this._ClientRepo.Find(this._LoginRepo.Find(sessionID).IDClient);

            client.UTCLastBackupTime = DateTime.UtcNow;
            this._ClientRepo.Update(client);

            this._BackupRepository.Update(backup);
        }

        public List<BackupInfo> GetLastTeplateBackupSet(long TemplateID)
        {
            long id = this._BackupRepository.GetLastTemplateBackup(TemplateID).ID;
            return this._BackupRepository.GetPreviousBackups(id);
        }
        #endregion

        #region COM actions
        public bool HasConfigUpdate(Guid sessionID, DateTime timestamp)
        {
            Client client = this._ClientRepo.Find(this._LoginRepo.Find(sessionID).IDClient);            
            
            return timestamp < client.UTCLastConfigUpdate;
        }

        public void ClientReportUpdated(Guid sessionID)
        {
            Client client = this._ClientRepo.Find(this._LoginRepo.Find(sessionID).IDClient);

            client.UTCLastStatusReportTime = DateTime.UtcNow;
            this._ClientRepo.Update(client);
        }
        #endregion
    }
}