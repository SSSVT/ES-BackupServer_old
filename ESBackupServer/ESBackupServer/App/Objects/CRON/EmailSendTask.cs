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
    public class EmailSendTask : IJob
    {
        #region Repositories

        private BackupRepository _BackupRepository { get; set; } = BackupRepository.GetInstance();
        private AdministratorRepository _AdministratorRepository { get; set; }
        private MailSender _MailSender { get; set; } = new MailSender();
        private MailFactory _MailFactory { get; set; } = new MailFactory();
        #endregion

        public void Execute(IJobExecutionContext context)
        {
            //TODO: Implement email sending

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