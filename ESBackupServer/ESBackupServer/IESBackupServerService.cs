﻿using ESBackupServer.App.Objects;
using ESBackupServer.App.Objects.Registration;
using System;
using System.ServiceModel;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IESBackupServerService" in both code and config file together.
    [ServiceContract]
    public interface IESBackupServerService
    {
        #region Registration
        UserDefinition RequestRegistration(string name, string hwid);
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
