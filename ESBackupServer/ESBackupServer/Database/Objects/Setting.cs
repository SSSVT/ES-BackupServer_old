using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClientSetting")]
    public class Setting
    {
        [Key, Column("ID")]
        public Guid ID { get; set; }

        [Column("IDesbk_tbClients")]
        public int IDClient { get; set; }

        [Column("IDesbk_tbClientSettingTypes")]
        public int IDSettingType { get; set; }

        [Column("ST_ACTION_TYPE")]
        public bool? ActionType { get; set; }

        [Column("ST_EVENT")]
        public bool? Event { get; set; }

        [Column("ST_TIME")]
        public DateTime Time { get; set; }

        [Column("ST_VALUE")]
        public string Value { get; set; }

        [ForeignKey("IDClient")]
        public virtual Client Client { get; set; }

        [ForeignKey("IDSettingType")]
        public virtual SettingType SettingType { get; set; }
    }
}