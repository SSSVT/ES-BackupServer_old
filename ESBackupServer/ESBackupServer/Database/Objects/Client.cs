using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClients")]
    public class Client
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Column("CL_NAME")]
        public string Name { get; set; }

        [Column("CL_DESCRIPTION")]
        public string Description { get; set; }

        [Column("CL_HWID")]
        public string Hardware_ID { get; set; }

        [Column("CL_LOGIN_NAME")]
        public string Login_Name { get; set; }

        [Column("CL_LOGIN_PSWD")]
        public string Login_Password { get; set; }

        [Column("CL_LOGIN_SALT")]
        public string Login_Salt { get; set; }

        [Column("CL_LAST_BACKUP")]
        public DateTime? LastBackup { get; set; }

        [Column("CL_VERIFIED")]
        public bool Verified { get; set; }

        public virtual List<Backup> Backups { get; set; }
        public virtual List<Log> Logs { get; set; }
        public virtual List<Login> Logins { get; set; }
        public virtual List<Setting> Settings { get; set; }
    }
}