using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackupTemplatesSetting")]
    public class BackupTemplateSetting
    {
        [Key, Column("ID")]
        public Guid ID { get; set; }

        [Column("IDesbk_tbBackupTemplates")]
        public long IDTemplate { get; set; }

        [Column("IDesbk_tbBackupTemplatesSettingTypes")]
        public int IDSettingType { get; set; }

        [Column("ST_ACTION_TYPE")]
        public bool? ActionType { get; set; }

        [Column("ST_EVENT")]
        public bool? Event { get; set; }

        [Column("ST_CRON")]
        public string CRON { get; set; }

        [Column("ST_VALUE")]
        public string Value { get; set; }

        #region Virtual properties
        [ForeignKey("IDTemplate")]
        public virtual BackupTemplate Template { get; set; }

        [ForeignKey("IDSettingType")]
        public virtual BackupTemplateSettingType SettingType { get; set; }
        #endregion
    }
}