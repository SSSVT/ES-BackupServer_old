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

        //TODO: Move to backup template || add link to template in objects
        #region Edit
        [DataMember]
        public List<EventDefinition> Events { get; set; }
        [DataMember]
        public List<TimeActionDefinition> TimeActions { get; set; }
        #endregion
    }
}