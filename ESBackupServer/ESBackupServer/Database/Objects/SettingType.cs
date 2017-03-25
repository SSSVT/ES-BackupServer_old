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
        public string Name { get; set; }

        public virtual List<Setting> Settings { get; set; }
    }
}