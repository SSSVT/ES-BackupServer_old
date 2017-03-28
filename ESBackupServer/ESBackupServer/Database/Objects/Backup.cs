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
        public Client IDClient { get; set; }

        [Column("BK_NAME"), DataMember]
        public string Name { get; set; }

        [Column("BK_DESCRIPTION"), DataMember]
        public string Description { get; set; }

        [Column("BK_TIME_BEGIN"), DataMember]
        public DateTime TimeStart { get; set; }

        [Column("BK_TIME_END"), DataMember]
        public DateTime? TimeEnd { get; set; }

        [Column("BK_EXPIRATION"), DataMember]
        public DateTime? ExpirationDate { get; set; }

        [Column("BK_COMPRESSION"), DataMember]
        public bool Compressed { get; set; }

        #region Virtual properties
        [ForeignKey("IDClient"), DataMember]
        public virtual Client Client { get; set; }

        [DataMember]
        public virtual List<Log> Logs { get; set; }
        #endregion
        #endregion
    }
}