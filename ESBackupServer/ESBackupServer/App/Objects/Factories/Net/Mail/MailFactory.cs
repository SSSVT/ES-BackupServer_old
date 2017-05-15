using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.App.Objects.Factories.Net.Mail
{
    public class MailFactory
    {
        private ClientRepository _ClientRepo { get; set; } = ClientRepository.GetInstance();
        private BackupRepository _BackupRepo { get; set; } = BackupRepository.GetInstance();

        public string CreateBody(List<BackupInfo> list)
        {
            int executing = list.Where(x => x.Status == 0).Count();
            int completed = list.Where(x => x.Status == 1).Count();
            int failed = list.Where(x => x.Status == 2).Count();

            return $"Completed backups: { completed } \nExecuting backups: { executing }\nFailed backups: { failed }\n\nGenerated at (UTC): { DateTime.UtcNow }";
        }

        public string CreateSubject()
        {
            return $"Report from ES Backup Server\n\n\n";
        }
    }
}