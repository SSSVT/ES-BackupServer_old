using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Column("LG_TIME_UTC"), DataMember] //, DatabaseGenerated(DatabaseGeneratedOption.Identity)
        public DateTime UTCTime { get; set; } = DateTime.UtcNow;

        //TODO: Default value
        [Column("LG_TIME_EXPIRATION_UTC")] //, DatabaseGenerated(DatabaseGeneratedOption.Identity)
        public DateTime UTCExpiration { get; set; } = DateTime.UtcNow.AddMinutes(15);

        [Column("LG_CLIENT_IP"), DataMember]
        public byte[] IP { get; set; } //IPv4 - 32; IPv6 - 128

        //TODO: Zkontrolovat, zda je potřebné
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