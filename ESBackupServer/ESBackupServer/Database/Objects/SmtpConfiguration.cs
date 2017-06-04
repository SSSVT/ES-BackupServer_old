using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_MailConfig"), DataContract]
    public class SmtpConfiguration
    {
        [Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("MC_SERVER")]
        public string Server { get; set; }

        [Column("MC_PORT")]
        public int Port { get; set; }

        [Column("MC_USERNAME")]
        public string Username { get; set; }

        [Column("MC_PASSWORD")]
        public string Password { get; set; }

        [Column("MC_FROM")]
        public string From { get; set; }

        [Column("MC_METHOD")]
        public int Method { get; set; }

        [Column("MC_PROTOCOL")]
        public int Protocol { get; set; }

        [Column("MC_DEFAULT")]
        public bool IsDefault { get; set; }
    }
}