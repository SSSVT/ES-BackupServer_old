using ESBackupServer.App.Objects;
using ESBackupServer.App.Objects.Net;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ESBackupServerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ESBackupServerService.svc or ESBackupServerService.svc.cs at the Solution Explorer and start debugging.
    public class ESBackupServerService : IESBackupServerService
    {
        #region User authentication
        /// <summary>
        /// Returns session ID
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Guid? Login(string username, string password)
        {
            ClientRepository ClientRepo = ClientRepository.GetInstance();
            Client client = ClientRepo.FindByUsername(username);

            if (ClientRepo.IsLoginValid(client, password))
            {
                Guid sessionID = LoginRepository.GetInstance().Create(client).ID;
                //ID, IP, UTC Time
                LogRepository.GetInstance().Create(client, $"Session start: ID={ sessionID };IP={ new NetInfoObtainer().GetClientIP().ToString() };UTCTime={ DateTime.UtcNow }", LogTypeNames.Message);
                return sessionID;
            }
            else
            {
                //IP, UTC Time
                LogRepository.GetInstance().Create(client, $"Invalid login: IP={ new NetInfoObtainer().GetClientIP().ToString() };UTCTime={ DateTime.UtcNow }", LogTypeNames.Warning);
                return null;
            } 
        }
        public bool Logout(Guid sessionID)
        {
            LoginRepository repo = LoginRepository.GetInstance();
            Login login = repo.Find(sessionID);
            login.Active = false;
            repo.SaveChanges();

            //ID, UTC Time
            LogRepository.GetInstance().Create(login.Client, $"Session end: ID={ sessionID };UTCTime={ DateTime.UtcNow }", LogTypeNames.Message);

            return true;
        }
        #endregion

        #region Debugging methods
        public string TestConnection()
        {
            return "Connection OK";
        }
        #endregion
    }
}