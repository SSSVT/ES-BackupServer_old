using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_MailConfig"), DataContract]
    public class SmtpConfiguration
    {
        [Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), DataMember]
        public int ID { get; set; }

        [Column("MC_SERVER"), DataMember]
        public string Server { get; set; }

        [Column("MC_PORT"), DataMember]
        public int Port { get; set; }

        [Column("MC_USERNAME"), DataMember]
        public string Username { get; set; }

        [Column("MC_PASSWORD"), DataMember]
        public string Password { get; set; }

        [Column("MC_METHOD"), DataMember]
        public int Method { get; set; }

        [Column("MC_PROTOCOL"), DataMember]
        public int Protocol { get; set; }

        [Column("MC_ACTIVE"), DataMember]
        public bool IsActive { get; set; }
    }
}