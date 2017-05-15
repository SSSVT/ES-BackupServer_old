using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackups"), DataContract(IsReference = true)]
    public class BackupInfo
    {
        #region Entity Framework
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), DataMember]
        public long ID { get; set; }

        [Column("IDesbk_tbClients"), DataMember]
        public int IDClient { get; set; }

        [Column("IDesbk_tbBackupTemplates"), DataMember]
        public long IDBackupTemplate { get; set; }

        [Column("BK_NAME"), DataMember]
        public string Name { get; set; }

        [Column("BK_DESCRIPTION"), DataMember]
        public string Description { get; set; }

        [Column("BK_TYPE"), DataMember]
        public byte BackupType { get; set; }

        [Column("IDesbk_tbBackups_BASE"), DataMember]
        public long? BaseBackupID { get; set; }

        [Column("BK_SOURCE"), DataMember]
        public string Source { get; set; }

        [Column("BK_DESTINATION"), DataMember]
        public string Destination { get; set; }
        
        [Column("BK_EXPIRATION_UTC"), DataMember]
        public DateTime? UTCExpiration { get; set; }

        [Column("BK_COMPRESSION"), DataMember]
        public bool Compressed { get; set; }

        [Column("BK_TIME_BEGIN_UTC"), DataMember]
        public DateTime UTCStart { get; set; }

        [Column("BK_TIME_END_UTC"), DataMember]
        public DateTime? UTCEnd { get; set; }

        [Column("BK_STATUS"), DataMember]
        public byte Status { get; set; }

        [Column("BK_META_PATH_ORDER"), DataMember]
        public UInt16 PathOrder { get; set; }

        [Column("BK_META_EMAIL_SENT"), DataMember]
        public bool EmailSent { get; set; }
        #endregion
        #region Virtual properties
        [ForeignKey("IDClient"), NotMapped]
        public virtual Client Client { get; set; }
        #endregion

        public BackupInfo()
        {
            this.BackupType = 0;
            this.Compressed = false;
            this.UTCStart = DateTime.UtcNow;
            this.Status = 0;
            this.EmailSent = false;
        }
    }
}