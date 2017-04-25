using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClientSettingTypes"), DataContract(IsReference = true)]
    public class BackupTemplateSettingType
    {
        [Key, Column("ID"), DataMember]
        public int ID { get; set; }

        [Column("TP_NAME"), DataMember]
        public SettingTypeNames Name { get; set; }

        public virtual List<BackupTemplateSetting> Settings { get; set; }
    }

    [DataContract]
    public enum SettingTypeNames
    {
        [EnumMember]
        Ignore = 1,
        [EnumMember]
        Only,

        [EnumMember]
        ShutDown,
        [EnumMember]
        Restart,
        [EnumMember]
        Sleep,
        [EnumMember]
        Hibernate,
        [EnumMember]
        Lock
    }
}