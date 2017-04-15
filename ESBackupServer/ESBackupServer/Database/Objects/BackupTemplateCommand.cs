using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackupTemplatesCommands"),DataContract(IsReference = true)]
    public class BackupTemplateCommand
    {
        #region Entity Framework        
        [Column("ID"), DataMember]
        public int ID { get; set; }
        [Column("IDesbk_tbBackupTemplates"), DataMember]
        public long IDTemplate { get; set; }
        [Column("CM_EVENT"), DataMember]
        public bool Event { get; set; }
        [Column("CM_TYPE"), DataMember]
        public bool Type { get; set; }
        #endregion
        #region Virtual properties        
        [ForeignKey("IDTemplate")]
        public virtual BackupTemplate Template { get; set; }
        #endregion
    }
}