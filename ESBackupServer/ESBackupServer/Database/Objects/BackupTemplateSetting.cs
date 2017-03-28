using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackupTemplatesSetting"), DataContract]
    public class BackupTemplateSetting
    {
        [Key, Column("ID")]
        public Guid ID { get; set; }

        [Column("IDesbk_tbBackupTemplates")]
        public long IDTemplate { get; set; }

        //TODO: Is it possible to use enum instead of int?
        [Column("IDesbk_tbBackupTemplatesSettingTypes")]
        public int IDSettingType { get; set; }

        [Column("ST_ACTION_TYPE")]
        public bool? ActionType { get; set; }

        [Column("ST_EVENT")]
        public bool? Event { get; set; }

        [Column("ST_TIME")]
        public DateTime? Time { get; set; }

        [Column("ST_VALUE")]
        public string Value { get; set; }

        #region Virtual properties
        [ForeignKey("IDClient")]
        public virtual Client Client { get; set; }

        [ForeignKey("IDSettingType")]
        public virtual SettingType SettingType { get; set; }
        #endregion
    }
}