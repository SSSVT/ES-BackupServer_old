using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbLogs"), DataContract(IsReference = true)]
    public class Log
    {
        #region Entity Framework
        [Key, Column("ID"), DataMember]
        public Guid ID { get; set; }

        [Column("IDesbk_tbClients"), DataMember]
        public int IDClient { get; set; }

        [Column("IDesbk_tbBackups"), DataMember]
        public long? IDBackup { get; set; }

        //TODO: Check
        [Column("IDesbk_tbLogTypes"), DataMember]
        public byte IDLogType { get; set; }

        [Column("LG_TIME_UTC"), DataMember]
        public DateTime UTCTime { get; set; }

        [Column("LG_VALUE"), DataMember]
        public string Value { get; set; }

        [ForeignKey("IDClient"), DataMember]
        public virtual Client Client { get; set; }

        [ForeignKey("IDBackup"), DataMember]
        public virtual Backup Backup { get; set; }

        [ForeignKey("IDLogType"), DataMember]
        public virtual LogType LogType { get; set; }
        #endregion

        public Log(Client client, DateTime UTCtime, string value, LogTypeNames logtype)
        {
            this.IDClient = client.ID;

            switch (logtype)
            {
                case LogTypeNames.Error:
                    this.IDLogType = 1;
                    break;
                case LogTypeNames.Warning:
                    this.IDLogType = 2;
                    break;
                case LogTypeNames.Message:
                    this.IDLogType = 3;
                    break;
            }

            this.UTCTime = UTCtime;
            this.Value = value;
        }
        public Log(Client client, Backup backup, DateTime UTCtime, string value, LogTypeNames logtype) : this(client, UTCtime, value, logtype)
        {
            this.IDBackup = backup.ID;
        }
        //TODO: Check
        public Log()
        {

        }
    }
}