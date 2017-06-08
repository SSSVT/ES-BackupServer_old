using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbLogins"), DataContract(IsReference = true)]
    public class Login
    {
        #region EntityFramework
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), DataMember]
        public Guid ID { get; set; }

        [Column("IDesbk_tbClients"), DataMember]
        public int IDClient { get; set; }

        [Column("LG_TIME_UTC"), DataMember]
        public DateTime UTCTime { get; set; }

        [Column("LG_TIME_EXPIRATION_UTC")]
        public DateTime UTCExpiration { get; set; }

        [Column("LG_CLIENT_IP"), DataMember]
        public string IP { get; set; } //IPv4 - 32; IPv6 - 128
        #endregion

        public Login()
        {
            this.UTCTime = DateTime.UtcNow;
            this.UTCExpiration = DateTime.UtcNow.AddMinutes(15);
        }
        public Login(Client item, DateTime UTCtime, IPAddress ip) : this()
        {
            this.IDClient = item.ID;
            this.UTCTime = UTCtime;
            this.IP = ip.ToString();
        }
    }
}