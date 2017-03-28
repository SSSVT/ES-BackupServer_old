using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ESBackupServer.Database.Objects
{
    [Table("esbk_tbClients"), DataContract]
    public class Client
    {
        //TODO: Add list of backup templates

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

        [Column("IDesbk_tbBackups_LAST_FULL"), DataMember]
        public long? IDLastFullBackup { get; set; }

        [Column("IDesbk_tbBackups_LAST_DIFF"), DataMember]
        public long? IDLastDifferentialBackup { get; set; }

        [Column("CL_VERIFIED"), DataMember]
        public bool Verified { get; set; }
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
        [ForeignKey("IDLastFullBackup"), DataMember]
        public virtual Backup LastFullBackup { get; set; }

        [ForeignKey("IDLastDifferentialBackup"), DataMember]
        public virtual Backup LastDifferentialBackup { get; set; }
        #region Lists
        [DataMember]
        public virtual List<Backup> Backups { get; set; }
        [DataMember]
        public virtual List<Log> Logs { get; set; }
        [DataMember]
        public virtual List<Login> Logins { get; set; }
        #endregion
        #endregion
        #endregion

        #region Methods
        public override string ToString()
        {
            return this.Name;
        }
        #endregion

        #region Getters
        public DateTime? LastBackupTime
        {
            get
            {
                if (this.LastFullBackup == null)
                    return null;
                else if (this.LastDifferentialBackup != null)
                    return this.LastDifferentialBackup.TimeEnd;
                return this.LastFullBackup.TimeEnd;
            }
        }
        #endregion
    }
}