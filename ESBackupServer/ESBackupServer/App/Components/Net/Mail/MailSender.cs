using System;
using System.Net;
using System.Net.Mail;

namespace ESBackupServer.App.Components.Net.Mail
{
    internal class MailSender
    {
        public void Send(string server, string recipient, string subject, string message, bool EnableSSL = false)
        {
            this.Send(
                new SmtpClient(server)
                {
                    EnableSsl = EnableSSL
                },
                new MailMessage(Properties.Settings.Default.MailSender, recipient, subject, message));
        }
        public void Send(string server, string username, string password, string recipient, string subject, string message, bool EnableSSL = false)
        {
            this.Send(
                new SmtpClient(server)
                {
                    EnableSsl = EnableSSL,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(username, password)
                },
                new MailMessage(Properties.Settings.Default.MailSender, recipient, subject, message));
        }

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