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
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), DataMember]
        public long ID { get; set; }

        [Column("IDesbk_tbClients"), DataMember]
        public int IDClient { get; set; }

        [Column("BK_NAME"), DataMember]
        public string Name { get; set; }

        [Column("BK_DESCRIPTION"), DataMember]
        public string Description { get; set; }

        [Column("BK_TYPE"), DataMember]
        public byte BackupType { get; set; } //TODO: enum

        [Column("BK_EXPIRATION_DAYS"), DataMember]
        public uint? DaysToExpiration { get; set; }

        [Column("BK_COMPRESSION"), DataMember]
        public bool Compression { get; set; }

        [Column("BK_SEARCH_PATTERN"), DataMember]
        public string SearchPattern { get; set; }

        [Column("BK_COPY_EMPTY_DIRS"), DataMember]
        public bool BackupEmptyDirectories { get; set; }

        [Column("BK_ENABLED"), DataMember]
        public bool Enabled { get; set; }

        [Column("BK_NOTIFICATION_ENABLED"), DataMember]
        public bool IsNotificationEnabled { get; set; }

        [Column("BK_NOTIFICATION_EMAIL_ENABLED"), DataMember]
        public bool IsEmailNotificationEnabled { get; set; }

        //TODO: CRON default value
        [Column("BK_REPEAT_INTERVAL_CRON"), DataMember]
        public string CRONRepeatInterval { get; set; }
        #endregion

        [NotMapped, DataMember]
        public List<BackupTemplatePath> Paths { get; set; }

        public BackupTemplate()
        {
            this.BackupType = 0;
            this.Compression = false;
            this.SearchPattern = "*";
            this.BackupEmptyDirectories = true;
            this.Enabled = false;
            this.IsNotificationEnabled = false;
            this.IsEmailNotificationEnabled = true;            
        }
    }
}