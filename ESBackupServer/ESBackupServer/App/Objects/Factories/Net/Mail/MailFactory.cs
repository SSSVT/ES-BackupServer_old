using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;
using System.Linq;

namespace ESBackupServer.App.Objects.Factories.Net.Mail
{
    public class MailFactory
    {
        private ClientRepository _ClientRepo { get; set; } = ClientRepository.GetInstance();
        private BackupRepository _BackupRepo { get; set; } = BackupRepository.GetInstance();

        public string CreateBody(Client client)
        {
            int executing = this._BackupRepo.FindByClientID(client.ID).Where(x => x.Status == 0).Count();
            int completed = this._BackupRepo.FindByClientID(client.ID).Where(x => x.Status == 1).Count();
            int failed = this._BackupRepo.FindByClientID(client.ID).Where(x => x.Status == 2).Count();

            return $"Client name: {client.Name} \nClient description: { client.Description } \nCompleted backups: { completed } \nExecuting backups: { executing }\nFailed backups: { failed }\n\nGenerated at (UTC): { DateTime.UtcNow }";
        }

        public string CreateSubject(Client client)
        {
            return $"Report from ES Backup Server";
        }
    }
}