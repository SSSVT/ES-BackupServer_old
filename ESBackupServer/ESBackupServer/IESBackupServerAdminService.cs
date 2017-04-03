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
        //TODO: Implement https://www.codeproject.com/Articles/763271/Common-issues-in-WCF

        #region Get
        [OperationContract]
        List<Client> GetClients();

        //potřeba?
        [OperationContract]
        List<Backup> GetBackups(Client client);

        //potřeba?
        [OperationContract]
        List<Log> GetLogs(Client client);

        //potřeba?
        [OperationContract]
        List<Log> GetLogs(Backup backup);

        [OperationContract]
        Configuration GetConfiguration(Client client);
        #endregion
        #region Set
        [OperationContract]
        bool SaveConfiguration(Client client, Configuration config);
        #endregion
    }
}
