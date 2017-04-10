using ESBackupServer.App.Objects;
using ESBackupServer.App.Objects.Filters;
using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

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
        Configuration GetConfiguration(Client client);
        #endregion
        #region Set
        [OperationContract]
        bool SaveConfiguration(Configuration config);
        #endregion
    }
}
