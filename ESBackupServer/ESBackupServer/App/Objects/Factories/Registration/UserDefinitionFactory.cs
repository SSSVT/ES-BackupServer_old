﻿using ESBackupServer.App.Objects.Registration;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;

namespace ESBackupServer.App.Objects.Factories.Registration
{
    internal class UserDefinitionFactory
    {
        internal UserDefinition Create(Client client)
        {
            if (client.Status == Convert.ToByte(ClientStatus.Verified) && client.Username == null)
            {
                string password = new PasswordFactory().Generate(128);
                UserDefinition def = new UserDefinition()
                {
                    //TODO: Create username, password and salt
                    Username = client.ID.ToString(),
                    Password = password,
                    Status = ClientStatus.Verified
                };
                client.Username = client.ID.ToString();
                client.Password = password;
                ClientRepository.GetInstance().Update(client);
                return def;
            }
            else
            {
                return new UserDefinition()
                {
                    Status = this.GetStatus(client.Status)
                };
            }                
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