using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackupDetails")]
    public class BackupDetail
    {
        [Key, Column("ID")]
        public Guid ID { get; set; }

        [Column("IDesbk_tbBackups")]
        public long IDBackup { get; set; }

        [Column("BK_PATH_SOURCE")]
        public string Source { get; set; }

        [Column("BK_PATH_DESTINATION")]
        public string Destination { get; set; }

        [Column("BK_TIME")]
        public DateTime Time { get; set; }

        [Column("BK_LAST_CHANGE")]
        public string DateModified { get; set; }

        [Column("BK_HASH")]
        public string Hash { get; set; }

        [ForeignKey("IDBackup")]
        public virtual Backup Backup { get; set; }
    }
}