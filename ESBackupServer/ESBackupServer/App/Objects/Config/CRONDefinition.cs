using System.Runtime.Serialization;

namespace ESBackupServer.App.Objects.Config
{
    [DataContract]
    public class CRONDefinition
    {
        public string Value { get; set; }
    }
}