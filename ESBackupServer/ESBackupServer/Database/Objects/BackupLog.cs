using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackupLogs")]
    public class BackupLog
    {
        [Key, Column("ID")]
        public int ID { get; set; }
        [Column("IDesbk_tbBackups")]
        public int IDBackup { get; set; }
        [Column("LG_TIME")]
        public DateTime LogDate { get; set; }
        [Column("LG_VALUE")]
        public string Value { get; set; }
        [ForeignKey("IDBackup")]
        public virtual Backup Backup { get; set; }
    }
}