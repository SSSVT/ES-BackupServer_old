using ESBackupServer.Database.Objects;
using System.Runtime.Serialization;

namespace ESBackupServer.App.Objects.Config
{
    public class CommandDefinition
    {
        [DataMember]
        public SettingTypeNames CommandType { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}