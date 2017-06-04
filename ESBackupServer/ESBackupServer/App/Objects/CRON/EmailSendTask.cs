using ESBackupServer.App.Components.Net.Mail;
using ESBackupServer.App.Objects.Factories.Net.Mail;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
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
        #endregion

        public void Execute(IJobExecutionContext context)
        {
            //TODO: Implement email sending

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
            }

            //foreach (Administrator admin in this._AdministratorRepository.FindAll())
            //{
            //    string message = this._MailFactory.CreateBody(this._BackupRepository.FindBackupsWithUnsentEmailByAdmin(admin.ID));

            //    this._MailSender.Send(
            //        "smtp.seznam.cz",
            //        "vlad.mrkacek@gmail.com",
            //        this._MailFactory.CreateSubject(),
            //        message);
            //}

        }
    }
}