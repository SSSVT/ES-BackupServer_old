using ESBackupServer.Database.Objects;
using System.Runtime.Serialization;

namespace ESBackupServer.App.Objects.Config
{
    [DataContract]
    public class CRONDefinition
    {
        [DataMember]
        public SettingTypeNames CommandType { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string CRON { get; set; }
    }
}