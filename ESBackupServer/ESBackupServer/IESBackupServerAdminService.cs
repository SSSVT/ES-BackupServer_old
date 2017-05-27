using ESBackupServer.App.Objects;
using ESBackupServer.App.Objects.Filters;
using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IESBackupServerAdminService" in both code and config file together.
    [ServiceContract]
    public interface IESBackupServerAdminService
    {
        #region Get
        [OperationContract]
        List<Client> GetClients(Filter filter, Sort sort);

        [OperationContract]
        Client GetClientByID(int ID);

        [OperationContract]
        List<BackupInfo> GetBackupsByClientID(int id);

        [OperationContract]
        BackupInfo GetBackupByID(long id);

        [OperationContract]
        List<Log> GetLogsByClientID(int id);

        [OperationContract]
        List<Log> GetLogsByBackupID(long id);

        [OperationContract]
        Configuration GetConfigurationByClientID(int id);

        [OperationContract]
        BackupTemplate GetTemplateByID(int id);

        //TODO: Rework to more efficent/safe way if possible
        [OperationContract]
        bool Login(string username, string password);

        [OperationContract]
        Administrator GetProfile(string username);

        [OperationContract]
        List<Login> GetLoginsByClient(int ID);

        #endregion
        #region Set
        [OperationContract]
        void UpdateClient(Client client);

        [OperationContract]
        void RemoveBackup(long id);

        [OperationContract]
        void UpdateBackup(BackupInfo backup);

        [OperationContract]
        void SaveTemplate(BackupTemplate template);

        [OperationContract]
        void SetTemplateStatus(long id, bool IsEnabled);

        [OperationContract]
        void RemoveBackupTemplate(long id);

        [OperationContract]
        void RemoveBackupTemplatePath(Guid id);

        [OperationContract]
        void UpdateAdministrator(Administrator admin);

        [OperationContract]
        void ClientConfigUpdated(int id);
        #endregion
    }
}
