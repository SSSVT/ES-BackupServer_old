using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClients"), DataContract(IsReference = true)]
    public class Client
    {
        #region Data members
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity), DataMember]
        public int ID { get; set; }

        [Column("IDesbk_tbAdministrators"), DataMember]
        public long IDAdministrator { get; set; }

        [Column("CL_NAME"), DataMember]
        public string Name { get; set; }

        [Column("CL_DESCRIPTION"), DataMember]
        public string Description { get; set; }

        [Column("CL_HWID")]
        public string HardwareID { get; set; }

        [Column("CL_LOGIN_NAME"), DataMember]
        public string Username { get; set; }

        [Column("CL_LOGIN_PSWD")]
        public string Password { get; set; }

        [Column("CL_STATUS"), DataMember]
        public byte Status { get; set; }

        [Column("CL_AUTO_STATUS_REPORT_ENABLED"), DataMember]
        public bool StatusReportEnabled { get; set; }

        [Column("CL_AUTO_STATUS_REPORT_INTERVAL"), DataMember]
        public int? ReportInterval { get; set; }

        [Column("CL_META_LAST_STATUS_REPORT_UTC"), DataMember]
        public DateTime? UTCLastStatusReportTime { get; set; }

        [Column("CL_META_LAST_BACKUP_UTC"), DataMember]
        public DateTime? UTCLastBackupTime { get; set; }

        [Column("CL_META_REGISTRATION_DATE_UTC"), DataMember]
        public DateTime UTCRegistrationDate { get; set; }
        #endregion

        public Client()
        {
            this.Status = 1;
            this.StatusReportEnabled = true;
            this.UTCRegistrationDate = DateTime.UtcNow;
            this.ReportInterval = 10*1000; //10 minut
        }
    }

    [DataContract]
    public enum ClientStatus
    {
        [EnumMember]
        Verified,
        [EnumMember]
        Unverified,        
        [EnumMember]
        Banned
    }
}