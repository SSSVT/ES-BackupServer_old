using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbLogins")]
    public class Login
    {
        #region EntityFramework
        [Key, Column("ID")]
        public Guid ID { get; set; }

        [Column("IDesbk_tbClients")]
        public int IDClient { get; set; }

        [Column("LG_TIME_UTC")]
        public DateTime UTCTime { get; set; }

        [Column("LG_CLIENT_IP")]
        public byte[] IP { get; set; } //IPv4 - 32; IPv6 - 128

        [Column("LG_ACTIVE")]
        public bool Active { get; set; }

        [ForeignKey("IDClient")]
        public virtual Client Client { get; set; }
        #endregion

        public Login(Client item, DateTime UTCtime)
        {
            this.IDClient = item.ID;
            this.UTCTime = UTCtime;
        }
    }
}