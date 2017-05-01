using ESBackupServer.Database.Objects;
using System.Runtime.Serialization;

namespace ESBackupServer.App.Objects.Registration
{
    [DataContract]
    public class RegistrationResponse
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public ClientStatus Status { get; set; }
    }
}