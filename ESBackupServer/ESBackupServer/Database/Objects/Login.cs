using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClientLogins")]
    public class Login
    {
        [Key, Column("ID")]
        public Guid ID { get; set; }
        [Column("IDesbk_tbClients")]
        public int IDClient { get; set; }

        [Column("LG_TIME ")]
        public DateTime Time { get; set; }

        [Column("LG_CLIENT_IP")]
        public byte[] IP { get; set; } //IPv4 - 32; IPv6 - 128

        [ForeignKey("IDClient")]
        public virtual Client Client { get; set; }
    }
}