using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbAdministrators"), DataContract(IsReference = true)]
    public class Administrator
    {
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), DataMember]
        public long ID { get; set; }

        [Column("AD_FIRST_NAME"), DataMember]
        public string FirstName { get; set; }

        [Column("AD_LAST_NAME"), DataMember]
        public string LastName { get; set; }

        [Column("AD_LOGIN_NAME")]
        public string Username { get; set; }

        [Column("AD_LOGIN_PSWD")]
        public string Password { get; set; }

        [Column("AD_META_REGISTRATION_DATE_UTC"), DataMember]
        public DateTime UTCRegistrationDate { get; set; }

        [NotMapped, DataMember]
        public List<Email> Emails { get; set; }

        public Administrator()
        {
            this.UTCRegistrationDate = DateTime.UtcNow;
        }
    }
}