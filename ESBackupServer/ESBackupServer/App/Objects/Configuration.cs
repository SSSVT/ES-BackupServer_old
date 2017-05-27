using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ESBackupServer.App.Objects
{
    [DataContract]
    public class Configuration
    {
        [DataMember]
        public List<BackupTemplate> Templates { get; set; }

        #region Client
        [DataMember]
        public bool StatusReportEnabled { get; set; }
        [DataMember]
        public int? ReportInterval { get; set; }
        #endregion

        #region Metadata
        [DataMember]
        public DateTime Generated { get; set; } = DateTime.UtcNow;
        #endregion
    }
}