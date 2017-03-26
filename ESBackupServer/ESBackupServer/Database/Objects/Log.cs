using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClientLogs")]
    public class Log
    {
        #region Entity Framework
        [Key, Column("ID")]
        public Guid ID { get; set; }

        [Column("IDesbk_tbClients")]
        public int IDClient { get; set; }

        [Column("IDesbk_tbBackups")]
        public long? IDBackup { get; set; }

        [Column("IDesbk_tbClientLogTypes")]
        public byte IDLogType { get; set; }

        [Column("LG_TIME")]
        public DateTime UTCTime { get; set; }

        [Column("LG_VALUE")]
        public string Value { get; set; }

        [ForeignKey("IDClient")]
        public virtual Client Client { get; set; }

        [ForeignKey("IDBackup")]
        public virtual Backup Backup { get; set; }

        [ForeignKey("IDLogType")]
        public virtual LogType LogType { get; set; }
        #endregion

        public Log(Client client, DateTime UTCtime, string value, LogTypeNames logtype)
        {
            this.IDClient = client.ID;

            switch (logtype)
            {
                case LogTypeNames.Error:
                    this.IDLogType = 0;
                    break;
                case LogTypeNames.Warning:
                    this.IDLogType = 1;
                    break;
                case LogTypeNames.Message:
                    this.IDLogType = 2;
                    break;
            }

            this.UTCTime = UTCtime;
            this.Value = value;
        }
        public Log(Client client, Backup backup, DateTime UTCtime, string value, LogTypeNames logtype) : this(client, UTCtime, value, logtype)
        {
            this.IDBackup = backup.ID;
        }
    }
}