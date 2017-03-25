using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClientLogs")]
    public class Log
    {
        [Key, Column("ID")]
        public Guid ID { get; set; }

        [Column("IDesbk_tbClients")]
        public int IDClient { get; set; }

        [Column("IDesbk_tbBackups")]
        public long? IDBackup { get; set; }

        [Column("IDesbk_tbClientLogTypes")]
        public byte IDLogType { get; set; }

        [Column("LG_TIME")]
        public DateTime Time { get; set; }

        [Column("LG_VALUE")]
        public string Value { get; set; }

        [ForeignKey("IDClient")]
        public virtual Client Client { get; set; }

        [ForeignKey("IDBackup")]
        public virtual Backup Backup { get; set; }

        [ForeignKey("IDLogType")]
        public virtual LogType LogType { get; set; }
    }
}