using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackupTemplates"), DataContract]
    public class BackupTemplate
    {
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
        public BackupTypes Type { get; set; }

        [Column("BK_EXPIRATION_DAYS"), DataMember]
        public uint? DaysToExpiration { get; set; }

        [Column("BK_COMPRESSION"), DataMember]
        public Compression Compression { get; set; }

        public virtual List<BackupTemplateSetting> Settings { get; set; }
    }

    [DataContract]
    public enum BackupTypes
    {
        [EnumMember]
        Full,
        [EnumMember]
        Differential
    }

    [DataContract]
    public enum Compression
    {
        Disabled,
        Enabled
    }
}