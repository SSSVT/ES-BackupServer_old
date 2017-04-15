using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackupTemplatesPathInfo"), DataContract(IsReference = true)]
    public class BackupTemplatePathInfo
    {
        #region Entity Framework
        [Column("ID"), DataMember]
        public int ID { get; set; }
        [Column("IDesbk_tbBackupTemplates"), DataMember]
        public long IDTemplate { get; set; }
        [Column("PI_SOURCE"), DataMember]
        public string Source { get; set; }
        [Column("PI_DESTINATION"), DataMember]
        public string Destination { get; set; }
        [Column("PI_RESTRICTION"), DataMember]
        public bool? Restriction { get; set; }
        [Column("PI_RESTRICTION_VALUE"), DataMember]
        public string RestrictionValue { get; set; }
        [Column("PI_TYPE"), DataMember]
        public bool Type { get; set; }
        #endregion
        #region Virtual properties
        [ForeignKey("IDTemplate"), DataMember]
        public virtual BackupTemplate Template { get; set; }
        #endregion

    }
}