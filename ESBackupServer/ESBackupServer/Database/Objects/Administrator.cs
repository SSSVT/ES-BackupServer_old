using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbAdministrators"), DataContract]
    public class Administrator
    {
        [Key, Column("ID"), DataMember]
        public long ID { get; set; }

        [Column("AD_FIRST_NAME"), DataMember]
        public string FirstName { get; set; }

        [Column("AD_LAST_NAME"), DataMember]
        public string LastName { get; set; }

        /* v2.0.0
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        */

        [Column("AD_REGISTRATION_DATE")]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}