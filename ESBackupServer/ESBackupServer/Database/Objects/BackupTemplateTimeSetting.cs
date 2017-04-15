using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackupTemplatesTimeSetting"),DataContract(IsReference =true)]
    public class BackupTemplateTimeSetting
    {
        #region Entity Framework
        [Column("ID"), DataMember]
        public int ID { get; set; }
        [Column("IDesbk_tbBackupTemplates"), DataMember]
        public long IDTemplate { get; set; }
        [Column("TS_TIME"), DataMember]
        public DateTime Time { get; set; }
        [Column("TS_REPEAT"), DataMember]
        public bool Repeat { get; set; }
        [Column("TS_REPEAT_CRON"), DataMember]
        public string CRON_Value { get; set; }
        #endregion

        #region Virtual properties
        [ForeignKey("IDTemplate")]
        public virtual BackupTemplate Template { get; set; }
        #endregion
    }
}