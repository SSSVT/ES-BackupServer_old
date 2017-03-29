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
        [Key, Column("ID"), DataMember]
        public Guid ID { get; set; }

        [Column("IDesbk_tbBackupTemplates"), DataMember]
        public long IDTemplate { get; set; }

        [Column("IDesbk_tbBackupTemplatesSettingTypes"), DataMember]
        public int IDSettingType { get; set; }

        [Column("ST_ACTION_TYPE"), DataMember]
        public bool? ActionType { get; set; }

        [Column("ST_EVENT"), DataMember]
        public bool? Event { get; set; }

        [Column("ST_TIME"), DataMember]
        public DateTime? Time { get; set; }

        [Column("ST_VALUE"), DataMember]
        public string Value { get; set; }

        #region Virtual properties
        [ForeignKey("IDClient"), DataMember]
        public virtual Client Client { get; set; }

        [ForeignKey("IDSettingType"), DataMember]
        public virtual BackupTemplateSettingType SettingType { get; set; }
        #endregion
    }
}