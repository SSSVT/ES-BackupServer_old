using ESBackupServer.App.Objects;
using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IESBackupServerService" in both code and config file together.
    [ServiceContract]
    public interface IESBackupServerService
    {
        [OperationContract]
        string TestConnection();

        #region Registration
        ClientStatus RequestRegistration(string name, string hwid);
        #endregion

        #region User authentication
        [OperationContract]
        Guid? Login(string username, string password);

        [OperationContract]
        bool Logout(Guid sessionID);
        #endregion

        #region Backup
        [OperationContract]
        Configuration GetConfiguration(Guid sessionID);
        #endregion
    }
}
