using ESBackupServer.App.Objects;
using ESBackupServer.App.Objects.Filters;
using ESBackupServer.Database.Objects;
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
        List<Backup> GetBackupsByClientID(int id);

        [OperationContract]
        Backup GetBackupByID(long id);

        [OperationContract]
        List<Log> GetLogsByClientID(int id);

        [OperationContract]
        List<Log> GetLogsByBackupID(long id);

        [OperationContract]
        Configuration GetConfigurationByClientID(int id);

        [OperationContract]
        BackupTemplate GetTemplateByID(int id);
        #endregion
        #region Set
        [OperationContract]
        void UpdateClient(Client client);

        [OperationContract]
        void RemoveBackup(long id);

        [OperationContract]
        void UpdateBackup(Backup backup);

        [OperationContract]
        void SaveTemplate(BackupTemplate template);

        [OperationContract]
        void SaveTemplatePath(BackupTemplatePath path);

        [OperationContract]
        void SetTemplateStatus(long id, bool IsEnabled);
        #endregion
    }
}
