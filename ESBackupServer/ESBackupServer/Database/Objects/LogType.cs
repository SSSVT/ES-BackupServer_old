using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClientLogTypes")]
    public class LogType
    {
        [Key, Column("ID")]
        public byte ID { get; set; }

        [Column("TP_NAME")]
        public string Name { get; set; }

        public virtual List<Log> Logs { get; set; }
    }
}