using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ESBackupServer.App.Objects.Config
{
    [DataContract]
    public class BackupSetting
    {
        public string Source { get; set; }
        public string Destination { get; set; }
    }
}