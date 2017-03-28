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
        [OperationContract]
        List<Client> GetClients();

        [OperationContract]
        List<Backup> GetBackups(Client client);

        [OperationContract]
        List<Log> GetLogs(Client client);

        [OperationContract]
        List<Log> GetLogs(Backup backup);
    }
}
