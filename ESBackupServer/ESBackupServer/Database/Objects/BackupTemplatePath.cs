using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackupTemplatesPaths"), DataContract(IsReference = true)]
    public class BackupTemplatePath
    {
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), DataMember]
        public Guid ID { get; set; }

        [Column("IDesbk_tbBackupTemplates"), DataMember]
        public long IDBackupTemplate { get; set; }

        [Column("BK_PATH_ORDER"), DataMember]
        public short PathOrder { get; set; }

        [Column("BK_TARGET_TYPE"), DataMember]
        public byte TargetType { get; set; } //0 = WIN, 1 = FTP, 2 = SSH, 3 = SecureCopy

        [Column("BK_SOURCE"), DataMember]
        public string Source { get; set; }

        [Column("BK_DESTINATION"), DataMember]
        public string Destination { get; set; }
    }
}