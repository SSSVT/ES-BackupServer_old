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
        public bool Type { get; set; }     

        [Column("BK_EXPIRATION_DAYS"), DataMember]
        public uint? DaysToExpiration { get; set; }

        [Column("BK_COMPRESSION"), DataMember]
        public bool Compression { get; set; }

        //TODO: Default value
        [Column("BK_SEARCH_PATTERN"), DataMember]
        public string SearchPattern { get; set; }

        [Column("BK_ENABLED"), DataMember]
        public bool Enabled { get; set; }

        [Column("BK_NOTIFICATION_ENABLED"), DataMember]
        public bool IsNotificationEnabled { get; set; }

        [Column("BK_NOTIFICATION_EMAIL_ENABLED"), DataMember]
        public bool IsEmailNotificationEnabled { get; set; }

        [Column("BK_REPEAT_INTERVAL_CRON"), DataMember]
        public string CRONRepeatInterval { get; set; }
        #endregion
    }
}