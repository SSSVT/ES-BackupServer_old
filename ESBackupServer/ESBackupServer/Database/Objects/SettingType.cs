using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClientSettingTypes")]
    public class SettingType
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Column("TP_NAME")]
        public SettingTypeNames Name { get; set; }

        public virtual List<Setting> Settings { get; set; }
    }

    public enum SettingTypeNames
    {
        Ingore,
        Only,
        PathSource,
        PathDestination,
        Compression,

        Start,
        Resume,
        Pause,
        Stop,

        ShutDown,
        Restart,
        Sleep,
        Hibernate,
        Lock,

        Email,
        Notification
    }
}