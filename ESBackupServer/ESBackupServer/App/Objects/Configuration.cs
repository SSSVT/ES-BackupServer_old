using ESBackupServer.App.Objects.Config;
using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ESBackupServer.App.Objects
{
    [DataContract]
    public class Configuration
    {
        [DataMember]
        public Client Client { get; set; }

        [DataMember]
        public List<BackupTemplate> Templates { get; set; }

        #region Metadata
        [DataMember]
        public DateTime Generated { get; set; } = DateTime.UtcNow;
        #endregion
    }
}