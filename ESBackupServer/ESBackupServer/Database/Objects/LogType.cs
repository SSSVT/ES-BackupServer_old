using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbLogTypes"), DataContract]
    public class LogType
    {
        #region DataMembers
        [Key, Column("ID"), DataMember]
        public byte ID { get; set; }

        [Column("TP_NAME"), DataMember]
        public LogTypeNames Name { get; set; }
        #endregion

        public virtual List<Log> Logs { get; set; }
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