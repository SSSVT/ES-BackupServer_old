using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbLogTypes")]
    public class LogType
    {
        [Key, Column("ID")]
        public byte ID { get; set; }

        [Column("TP_NAME")]
        public LogTypeNames Name { get; set; }

        public virtual List<Log> Logs { get; set; }
    }

    public enum LogTypeNames
    {
        Error = 1,
        Warning,
        Message
    }
}