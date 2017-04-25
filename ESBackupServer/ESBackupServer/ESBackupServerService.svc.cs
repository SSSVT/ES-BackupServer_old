﻿using ESBackupServer.App.Objects;
using ESBackupServer.App.Objects.Components.Net;
using ESBackupServer.App.Objects.Factories.Config;
using ESBackupServer.App.Objects.Factories.Registration;
using ESBackupServer.App.Objects.Registration;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ESBackupServerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ESBackupServerService.svc or ESBackupServerService.svc.cs at the Solution Explorer and start debugging.
    public class ESBackupServerService : IESBackupServerService
    {
        #region Properties
        #region Repos
        private ClientRepository _ClientRepo { get; set; } = ClientRepository.GetInstance();
        private LoginRepository _LoginRepo { get; set; } = LoginRepository.GetInstance();
        private LogRepository _LogRepo { get; set; } = LogRepository.GetInstance();
        #endregion
        #region Factories
        private ConfigurationFactory _ConfigFactory { get; set; } = new ConfigurationFactory();
        #endregion
        #endregion

        #region Registration
        public UserDefinition RequestRegistration(string name, string hwid)
        {
            Client item = this._ClientRepo.Find(name, hwid);
            if (item == null)
                item = this._ClientRepo.CreateClient(name, hwid);
            return new UserDefinitionFactory().Create(item);
        }
        #endregion

        #region User authentication
        /// <summary>
        /// Returns session ID
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Guid? Login(string username, string password)
        {
            Client client = this._ClientRepo.FindByUsername(username);

            if (this._ClientRepo.IsLoginValid(client, password))
            {
                Guid sessionID = this._LoginRepo.Create(client).ID;
                //ID, IP, UTC Time
                this._LogRepo.Create(client, $"Session start: ID={ sessionID };IP={ new NetInfoObtainer().GetClientIP().ToString() };UTCTime={ DateTime.UtcNow }", LogTypeNames.Message);
                return sessionID;
            }
            else
            {
                //IP, UTC Time
                this._LogRepo.Create(client, $"Invalid login: IP={ new NetInfoObtainer().GetClientIP().ToString() };UTCTime={ DateTime.UtcNow }", LogTypeNames.Warning);
                return null;
            }
        }
        public bool Logout(Guid sessionID)
        {
            Login login = this._LoginRepo.Find(sessionID);
            login.UTCExpiration = DateTime.UtcNow;
            this._LoginRepo.SaveChanges();

            //ID, UTC Time
            this._LogRepo.Create(login.Client, $"Session end: ID={ sessionID };UTCTime={ DateTime.UtcNow }", LogTypeNames.Message);

            return true;
        }
        #endregion

        #region Backup
        public Configuration GetConfiguration(Guid sessionID)
        {
            Login login = this._LoginRepo.Find(sessionID);
            return (this._LoginRepo.IsSessionIDValid(login)) ? this._ConfigFactory.Create(login.Client) : null;  
        }
        #endregion
    }
}