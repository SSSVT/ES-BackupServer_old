using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbBackups")]
    public class Backup
    {
        [Key, Column("ID")]
        public int ID { get; set; }
        [Column("BK_IDesbk_tbClients")]
        public Client IDClient { get; set; }
        [Column("BK_NAME")]
        public string Name { get; set; }
        [Column("BK_DESCRIPTION")]
        public string Description { get; set; }
        [Column("BK_TIME_BEGIN")]
        public DateTime Time_Begin { get; set; }
        [Column("BK_TIME_ENDs")]
        public DateTime? Time_End { get; set; }
        [Column("BK_EXPIRATION")]
        public DateTime? Expire_Date { get; set; }

        [ForeignKey("IDClient")]
        public virtual Client Client { get; set; }
    }
}