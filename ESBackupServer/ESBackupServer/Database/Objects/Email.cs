using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbEmails"), DataContract]
    public class Email
    {
        [Key, Column("ID")]
        public Guid ID { get; set; }

        [Column("IDesbk_tbAdministrators")]
        public long IDAdministrator { get; set; }

        [Column("EMAIL")]
        public string Address { get; set; }

        [Column("ISDEFAULT")]
        public bool IsDefault { get; set; }
    }
}