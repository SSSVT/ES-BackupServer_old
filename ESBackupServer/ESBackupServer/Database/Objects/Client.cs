using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClients")]
    public class Client
    {
        [Key, Column("ID")]
        public int ID { get; set; }
        [Column("CL_Name")]
        public string Name { get; set; }
        [Column("CL_DESCRIPTION")]
        public string Description { get; set; }
        [Column("CL_HWID")]
        public string Hardware_ID { get; set; }
        [Column("CL_LOGIN_NAME")]
        public string Login_Name { get; set; }
        [Column("CL_LOGIN_PASSWORD")]
        public string Login_Password { get; set; }
        [Column("CL_LOGIN_SALT")]
        public string Login_Salt { get; set; }
        [Column("CL_VERIFIED")]
        public bool Verified { get; set; }
        public virtual List<Backup> Backups { get; set; }
    }
}