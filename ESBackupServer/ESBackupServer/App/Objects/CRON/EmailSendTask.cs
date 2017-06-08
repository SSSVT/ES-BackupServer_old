using ESBackupServer.App.Components.Net.Mail;
using ESBackupServer.App.Objects.Factories.Net.Mail;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ESBackupServer.App.Objects.CRON
{
    internal class EmailSendTask : IJob
    {
        #region Repositories
        private MailSender _MailSender { get; set; } = new MailSender();
        private MailFactory _MailFactory { get; set; } = new MailFactory();

        protected BackupRepository _BackupRepository { get; set; } = new BackupRepository();
        protected ClientRepository _ClientRepository { get; set; } = new ClientRepository();
        protected AdministratorRepository _AdministratorRepository { get; set; } = new AdministratorRepository();
        protected EmailRepository _EmailRepository { get; set; } = new EmailRepository();
        protected SmtpConfigurationRepository _SmtpConfigurationRepository { get; set; } = new SmtpConfigurationRepository();
        #endregion

        public void Execute(IJobExecutionContext context)
        {
            SmtpConfiguration smtpconfig = this._SmtpConfigurationRepository.FindDefault();

            SmtpDeliveryMethod method = this.GetStmpDeliveryMethod(smtpconfig);
            SecurityProtocolType protocol = this.GetSecurityProtocolType(smtpconfig);

            foreach (Administrator admin in this._AdministratorRepository.FindAll())
            {
                long executing = 0;
                long completed = 0;
                long failed = 0;

                foreach (Client client in this._ClientRepository.FindByAdmin(admin.ID))
                {
                    executing += this._BackupRepository.GetCount(client.ID, 0);
                    completed += this._BackupRepository.GetCount(client.ID, 1);
                    failed += this._BackupRepository.GetCount(client.ID, 2);
                }

                string subject = this._MailFactory.CreateSubject();
                string message = this._MailFactory.CreateBody(executing, completed, failed);

                foreach (Email email in this._EmailRepository.Find(admin))
                {
                    this._MailSender.Send(smtpconfig.Server, smtpconfig.Port, smtpconfig.Username, smtpconfig.Password, smtpconfig.From, email.Address, subject, message, method, protocol);
                }
            }
        }

        protected SmtpDeliveryMethod GetStmpDeliveryMethod(SmtpConfiguration config)
        {
            switch (config.Method)
            {
                case 1:
                    return SmtpDeliveryMethod.SpecifiedPickupDirectory;
                case 2:
                    return SmtpDeliveryMethod.PickupDirectoryFromIis;
                default:
                    return SmtpDeliveryMethod.Network;
            }
        }

        protected SecurityProtocolType GetSecurityProtocolType(SmtpConfiguration config)
        {
            switch (config.Protocol)
            {
                case 192:
                    return SecurityProtocolType.Tls;
                case 768:
                    return SecurityProtocolType.Tls11;
                case 3072:
                    return SecurityProtocolType.Tls12;
                default:
                    return SecurityProtocolType.Ssl3;
            }
        }
    }
}