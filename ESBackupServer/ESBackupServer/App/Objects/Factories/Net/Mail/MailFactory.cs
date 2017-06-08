using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESBackupServer.App.Objects.Factories.Net.Mail
{
    public class MailFactory
    {
        public string CreateBody(long executing, long completed, long failed)
        {
            return $"Completed backups: { completed } \nExecuting backups: { executing }\nFailed backups: { failed }\n\nGenerated at (UTC): { DateTime.UtcNow }";
        }

        public string CreateSubject()
        {
            return $"Report from ES Backup Server";
        }
    }
}