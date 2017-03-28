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
        [Key, Column("ID")]
        public long ID { get; set; }

        [Column("IDesbk_tbClients")]
        public Client IDClient { get; set; }

        [Column("BK_NAME")]
        public string Name { get; set; }

        [Column("BK_DESCRIPTION")]
        public string Description { get; set; }

        [Column("BK_TIME_BEGIN")]
        public DateTime TimeStart { get; set; }

        [Column("BK_TIME_END")]
        public DateTime? TimeEnd { get; set; }

        [Column("BK_EXPIRATION")]
        public DateTime? ExpirationDate { get; set; }

        [Column("BK_COMPRESSION")]
        public bool Compressed { get; set; }

        [ForeignKey("IDClient")]
        public virtual Client Client { get; set; }

        public virtual List<BackupDetail> Details { get; set; }
    }
}