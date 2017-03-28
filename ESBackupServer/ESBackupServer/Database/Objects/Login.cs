using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbLogins"), DataContract]
    public class Login
    {
        #region EntityFramework
        [Key, Column("ID"), DataMember]
        public Guid ID { get; set; }

        [Column("IDesbk_tbClients"), DataMember]
        public int IDClient { get; set; }

        [Column("LG_TIME_UTC"), DataMember]
        public DateTime UTCTime { get; set; }

        [Column("LG_CLIENT_IP"), DataMember]
        public byte[] IP { get; set; } //IPv4 - 32; IPv6 - 128

        [Column("LG_ACTIVE"), DataMember]
        public bool Active { get; set; }

        [ForeignKey("IDClient"), DataMember]
        public virtual Client Client { get; set; }
        #endregion

        public Login(Client item, DateTime UTCtime)
        {
            this.IDClient = item.ID;
            this.UTCTime = UTCtime;
        }
    }
}