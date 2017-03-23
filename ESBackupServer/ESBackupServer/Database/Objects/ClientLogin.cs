using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClientLogins")]
    public class ClientLogin
    {
        [Key, Column("ID")]
        public int ID { get; set; }
        [Column("IDesbk_tbClients")]
        public int IDClient { get; set; }

        [Column("LG_TIME ")]
        public DateTime Log_Time { get; set; }
    }
}