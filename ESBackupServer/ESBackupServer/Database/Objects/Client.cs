using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClients"), DataContract(IsReference = true)]
    public class Client
    {
        #region Entity Framework
        #region Data members
        [Key, Column("ID"), DataMember]
        public int ID { get; set; }

        [Column("CL_NAME"), DataMember]
        public string Name { get; set; }

        [Column("CL_DESCRIPTION"), DataMember]
        public string Description { get; set; }

        [Column("CL_LOGIN_NAME"), DataMember]
        public string Username { get; set; }

        [Column("CL_LAST_BACKUP"), DataMember]
        public DateTime? LastBackupTime { get; set; }

        [Column("CL_STATUS"), DataMember]
        public byte Status { get; set; }

        [Column("CL_AUTO_STATUS_REPORT_ENABLED"), DataMember]
        public bool StatusReportEnabled { get; set; }

        /// <summary>
        /// In milisecond
        /// </summary>
        [Column("CL_AUTO_STATUS_REPORT_INTERVAL"), DataMember]
        public int? ReportInterval { get; set; }

        [Column("CL_LAST_STATUS_REPORT"), DataMember]
        public DateTime? LastReportTime { get; set; }
        #endregion
        #region Not data member
        [Column("CL_HWID")]
        public string Hardware_ID { get; set; }

        [Column("CL_LOGIN_PSWD")]
        public string Password { get; set; }

        [Column("CL_LOGIN_SALT")]
        public string Salt { get; set; }
        #endregion

        #region Virtual properties
        [DataMember]
        public virtual List<Backup> Backups { get; set; }

        [DataMember]
        public virtual List<Log> Logs { get; set; }

        [DataMember]
        public virtual List<Login> Logins { get; set; }

        [DataMember]
        public virtual List<BackupTemplate> Templates { get; set; }
        #endregion
        #endregion
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