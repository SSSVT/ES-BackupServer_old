using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackupTemplates"), DataContract(IsReference = true)]
    public class BackupTemplate
    {
        #region Entity Framework
        [Key, Column("ID"), DataMember]
        public long ID { get; set; }

        [Column("IDesbk_tbClients"), DataMember]
        public int IDClient { get; set; }

        [Column("BK_NAME"), DataMember]
        public string Name { get; set; }

        [Column("BK_DESCRIPTION"), DataMember]
        public string Description { get; set; }

        [Column("BK_SOURCE"), DataMember]
        public string Source { get; set; }

        [Column("BK_DESTINATION"), DataMember]
        public string Destination { get; set; }

        [Column("BK_TYPE"), DataMember]
        public bool Type { get; set; }

        [Column("BK_EXPIRATION_DAYS"), DataMember]
        public uint? DaysToExpiration { get; set; }

        [Column("BK_COMPRESSION"), DataMember]
        public bool Compression { get; set; }

        [Column("BK_ENABLED"), DataMember]
        public bool Enabled { get; set; }

        [Column("BK_NOTIFICATION_ENABLED"), DataMember]
        public bool IsNotificationEnabled { get; set; }

        [Column("BK_REPEAT_INTERVAL_CRON"), DataMember]
        public string CRONRepeatInterval { get; set; }

        [Column("BK_SEARCH_PATTERN"), DataMember]
        public string SearchPattern { get; set; }

        [ForeignKey("IDClient"), DataMember]
        public virtual Client Client { get; set; }

        [DataMember]
        public virtual List<Backup> Backups { get; set; }

        public virtual List<BackupTemplateSetting> Settings { get; set; }
        #endregion
    }
}