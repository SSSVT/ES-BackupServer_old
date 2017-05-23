using ESBackupServer.App.Objects;
using ESBackupServer.App.Objects.Authentication;
using ESBackupServer.App.Objects.Registration;
using ESBackupServer.Database.Objects;
using System;
using System.ServiceModel;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IESBackupServerService" in both code and config file together.
    [ServiceContract]
    public interface IESBackupServerService
    {
        #region Registration
        [OperationContract]
        RegistrationResponse RequestRegistration(string name, string hwid);
        #endregion

        #region User authentication
        [OperationContract]
        LoginResponse Login(string username, string password);

        [OperationContract]
        bool Logout(Guid sessionID);
        #endregion

        #region Backup
        [OperationContract]
        Configuration GetConfiguration(Guid sessionID);

        [OperationContract]
        void CreateBackup(BackupInfo backup, Guid sessionID);
        #endregion

        #region COM actions
        [OperationContract]
        bool HasConfigUpdate(Guid sessionID, DateTime timestamp);
        [OperationContract]
        void ClientReportUpdated(Guid sessionID);
        //TODO: CHECK
        #endregion
    }
}
