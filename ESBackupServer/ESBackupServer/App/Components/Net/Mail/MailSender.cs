using System;
using System.Net;
using System.Net.Mail;

namespace ESBackupServer.App.Components.Net.Mail
{
    internal class MailSender
    {
        #region Without security protocol
        public void Send(string server, int port, string from, string to, string subject, string message)
        {
            this.Send(server, port, from, to, subject, message, SmtpDeliveryMethod.Network);
        }
        public void Send(string server, int port, string from, string to, string subject, string message, SmtpDeliveryMethod method)
        {
            this.Send(
                new SmtpClient(server, port)
                {
                    DeliveryMethod = method
                },
                new MailMessage(from, to, subject, message));
        }

        public void Send(string server, int port, string username, string password, string from, string to, string subject, string message)
        {
            this.Send(server, port, username, password, from, to, subject, message, SmtpDeliveryMethod.Network);
        }
        public void Send(string server, int port, string username, string password, string from, string to, string subject, string message, SmtpDeliveryMethod method)
        {
            this.Send(
                new SmtpClient(server, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(username, password),
                    DeliveryMethod = method
                },
                new MailMessage(from, to, subject, message));
        }

        #endregion
        #region With security protocol
        public void Send(string server, int port, string from, string to, string subject, string message, SecurityProtocolType protocol)
        {
            this.Send(server, port, from, to, subject, message, SmtpDeliveryMethod.Network, protocol);
        }
        public void Send(string server, int port, string from, string to, string subject, string message, SmtpDeliveryMethod method, SecurityProtocolType protocol)
        {
            ServicePointManager.SecurityProtocol = protocol;
            this.Send(
                new SmtpClient(server, port)
                {
                    EnableSsl = true,
                    DeliveryMethod = method
                },
                new MailMessage(from, to, subject, message));
        }

        public void Send(string server, int port, string username, string password, string from, string to, string subject, string message, SecurityProtocolType protocol)
        {
            this.Send(server, port, username, password, from, to, subject, message, SmtpDeliveryMethod.Network, protocol);
        }
        public void Send(string server, int port, string username, string password, string from, string to, string subject, string message, SmtpDeliveryMethod method, SecurityProtocolType protocol)
        {
            ServicePointManager.SecurityProtocol = protocol;
            this.Send(
                new SmtpClient(server, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true,
                    DeliveryMethod = method
                },
                new MailMessage(from, to, subject, message));
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
                throw ex; //TODO: Fix potential bug --> log(client) - write to log
            }
        }
    }
}