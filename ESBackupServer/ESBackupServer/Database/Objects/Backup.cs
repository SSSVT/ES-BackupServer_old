using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackups"), DataContract]
    public class Backup
    {
        #region Entity Framework
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

        [Column("BK_EXPIRATION"), DataMember]
        public DateTime? Expiration { get; set; }

        [Column("BK_COMPRESSION"), DataMember]
        public bool Compressed { get; set; }

        [Column("BK_TIME_BEGIN"), DataMember]
        public DateTime Start { get; set; }

        [Column("BK_TIME_END"), DataMember]
        public DateTime? End { get; set; }

        [Column("BK_STATUS"), DataMember]
        public Statuses Status { get; set; }

        #region Virtual properties
        [ForeignKey("IDClient"), DataMember]
        public virtual Client Client { get; set; }

        [DataMember]
        public virtual List<Log> Logs { get; set; }
        #endregion
        #endregion

        [DataContract]
        public enum Statuses
        {
            [EnumMember]
            Executing,
            [EnumMember]
            Completed,
            [EnumMember]
            Failed
        }
    }
}