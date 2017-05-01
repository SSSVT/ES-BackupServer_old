using ESBackupServer.App.Objects;
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
        private BackupRepository _BackupRepository { get; set; } = BackupRepository.GetInstance();
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
                item = this._ClientRepo.CreateClient(AdministratorRepository.GetInstance().Find(1), name, hwid); //TODO: Remove debugging code
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
                Guid sessionID = this._LoginRepo.Create(client, this._NetInfo.GetClientIP()).ID;
                this._LogRepo.Create(client, $"Session start: ID={ sessionID };IP={ new NetInfoObtainer().GetClientIP().ToString() };UTCTime={ DateTime.UtcNow }", LogTypeNames.Message);
                return sessionID;
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

        public void CreateBackup(Backup backup)
        {
            this._BackupRepository.Update(backup);
        }
        #endregion
    }
}