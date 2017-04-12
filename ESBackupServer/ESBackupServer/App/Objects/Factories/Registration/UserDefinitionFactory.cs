using ESBackupServer.App.Objects.Registration;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;

namespace ESBackupServer.App.Objects.Factories.Registration
{
    internal class UserDefinitionFactory
    {
        private ClientRepository _ClientRepository { get; set; } = ClientRepository.GetInstance();

        internal UserDefinition Create(Client client)
        {
            this._ClientRepository.Update(client);
            return (client.Status == (int)ClientStatus.Verified)
                ? new UserDefinition()
                {
                    //TODO: Create username, password and salt
                    Username = "test",
                    Password = "password",
                    Status = ClientStatus.Verified
                }
                : new UserDefinition()
                {
                    Status = this.GetStatus(client.Status)
                };
        }

        private ClientStatus GetStatus(byte code)
        {
            switch (code)
            {
                case 0:
                    return ClientStatus.Verified;
                case 1:
                    return ClientStatus.Unverified;
                default:
                    return ClientStatus.Banned;
            }
        }
    }
}