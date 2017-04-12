using System;
using System.Net;
using System.Net.Mail;

namespace ESBackupServer.App.Components.Net.Mail
{
    internal class MailSender
    {
        #region Without security protocol
        public void Send(string server, string recipient, string subject, string message)
        {
            this.Send(new SmtpClient(server), new MailMessage(Properties.Settings.Default.MailSender, recipient, subject, message));
        }
        public void Send(string server, string username, string password, string recipient, string subject, string message)
        {
            this.Send(
                new SmtpClient(server)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(username, password)
                },
                new MailMessage(Properties.Settings.Default.MailSender, recipient, subject, message));
        }
        #endregion
        #region With security protocol
        public void Send(string server, string recipient, string subject, string message, SecurityProtocolType protocol)
        {
            ServicePointManager.SecurityProtocol = protocol;
            this.Send(
                new SmtpClient(server)
                {
                    EnableSsl = true
                },
                new MailMessage(Properties.Settings.Default.MailSender, recipient, subject, message));
        }
        public void Send(string server, string username, string password, string recipient, string subject, string message, SecurityProtocolType protocol)
        {
            ServicePointManager.SecurityProtocol = protocol;
            this.Send(
                new SmtpClient(server)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(username, password)
                },
                new MailMessage(Properties.Settings.Default.MailSender, recipient, subject, message));
        }
        #endregion

        private void Send(SmtpClient client, MailMessage message)
        {
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex; //TODO: Fix potential bug
            }
        }
    }
}