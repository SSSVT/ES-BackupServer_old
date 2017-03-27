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
        /// <summary>
        /// Sources and destinations
        /// </summary>
        public List<BackupSetting> Backups { get; set; }
        /// <summary>
        /// Times, when to start/stop/pause/resume backup
        /// </summary>
        public List<TimeSetting> Times { get; set; }


        #region Metadata
        public DateTime Expiration { get; set; }
        public bool Compression { get; set; }
        #endregion
    }
}