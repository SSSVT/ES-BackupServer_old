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
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), DataMember]
        public Guid ID { get; set; }

        [Column("IDesbk_tbClients"), DataMember]
        public int IDClient { get; set; }

        [Column("IDesbk_tbBackups"), DataMember]
        public long? IDBackup { get; set; }

        [Column("LG_TYPE"), DataMember]
        public byte LogType { get; set; }

        [Column("LG_TIME_UTC"), DataMember]
        public DateTime UTCTime { get; set; }

        [Column("LG_VALUE"), DataMember]
        public string Value { get; set; }
        #endregion

        #region Constructors        
        public Log()
        {
            this.UTCTime = DateTime.UtcNow;
        }
        public Log(Client client, DateTime UTCtime, string value, LogTypeNames logtype)
        {
            this.IDClient = client.ID;

            switch (logtype)
            {
                case LogTypeNames.Error:
                    this.LogType = 1;
                    break;
                case LogTypeNames.Warning:
                    this.LogType = 2;
                    break;
                case LogTypeNames.Message:
                    this.LogType = 3;
                    break;
            }

            this.UTCTime = UTCtime;
            this.Value = value;
        }
        public Log(Client client, Backup backup, DateTime UTCtime, string value, LogTypeNames logtype) : this(client, UTCtime, value, logtype)
        {
            this.IDBackup = backup.ID;
        }
        #endregion
    }

    [DataContract]
    public enum LogTypeNames
    {
        [EnumMember]
        Error = 1,
        [EnumMember]
        Warning,
        [EnumMember]
        Message
    }
}