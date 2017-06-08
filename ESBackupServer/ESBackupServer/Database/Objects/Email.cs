using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbEmails"), DataContract(IsReference = true)]
    public class Email
    {
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), DataMember]
        public Guid ID { get; set; }

        [Column("IDesbk_tbAdministrators"), DataMember]
        public long IDAdministrator { get; set; }

        [Column("EMAIL"), DataMember]
        public string Address { get; set; }

        [Column("ISDEFAULT"), DataMember]
        public bool IsDefault { get; set; }

        #region Virtual properties
        [ForeignKey("IDAdministrator"), NotMapped]
        internal virtual Administrator Administrator { get; set; }
        #endregion
        public Email()
        {
            this.IsDefault = false;
        }
    }
}