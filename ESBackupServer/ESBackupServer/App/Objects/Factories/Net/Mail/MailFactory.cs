using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;

namespace ESBackupServer.App.Objects.Factories.Net.Mail
{
    public class MailFactory
    {
        private ClientRepository _ClientRepo { get; set; } = ClientRepository.GetInstance();

        public string CreateBody(Client client)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }

        public string CreateSubject(Client client)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}