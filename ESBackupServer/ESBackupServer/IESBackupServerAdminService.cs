using ESBackupServer.App.Objects;
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
        List<Client> GetClients();

        //potřeba?
        [OperationContract]
        List<Backup> GetBackups(Client client);

        //potřeba?
        [OperationContract]
        List<Log> GetLogsByClient(Client client);

        //potřeba?
        [OperationContract]
        List<Log> GetLogsByBackup(Backup backup);

        [OperationContract]
        Configuration GetConfiguration(Client client);
        #endregion
        #region Set
        [OperationContract]
        bool SaveConfiguration(Configuration config);
        #endregion
    }
}
