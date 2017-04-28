using System;
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

        //TODO: Default value
        [Column("AD_META_REGISTRATION_DATE_UTC")] //, DatabaseGenerated(DatabaseGeneratedOption.Identity)
        public DateTime UTCRegistrationDate { get; set; } = DateTime.UtcNow;
    }
}