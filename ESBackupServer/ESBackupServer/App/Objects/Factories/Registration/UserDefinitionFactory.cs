using ESBackupServer.App.Objects.Registration;
using ESBackupServer.Database.Objects;
using System;

namespace ESBackupServer.App.Objects.Factories.Registration
{
    internal class UserDefinitionFactory
    {
        internal UserDefinition Create(Client client)
        {
            return (Convert.ToInt32(client.Status) == Convert.ToInt32(ClientStatus.Verified) && client.Username == null)
                ? new UserDefinition()
                {
                    //TODO: Create username, password and salt
                    Username = client.ID.ToString(),
                    Password = new PasswordFactory().Generate(128),
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