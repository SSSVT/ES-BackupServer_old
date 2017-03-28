using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ESBackupServer.App.Objects;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ESBackupServerAdminService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ESBackupServerAdminService.svc or ESBackupServerAdminService.svc.cs at the Solution Explorer and start debugging.
    public class ESBackupServerAdminService : IESBackupServerAdminService
    {
        #region Properties
        private BackupRepository _BackupRepository { get; set; } = BackupRepository.GetInstance();
        private ClientRepository _ClientRepository { get; set; } = ClientRepository.GetInstance();
        private LogRepository _LogRepository { get; set; } = LogRepository.GetInstance(); 
        #endregion

        public List<Backup> GetBackups(Client client)
        {
            return this._ClientRepository.Find(client.ID).Backups;
        }
        public List<Client> GetClients()
        {
            return this._ClientRepository.FindAll();
        }

        public Configuration GetConfiguration(Client client)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }

        public List<Log> GetLogs(Client client)
        {
            return this._ClientRepository.Find(client.ID).Logs;
        }

        //TODO: Implement
        /*
        public List<Log> GetLogs(Backup backup)
        {
            return this._BackupRepository.Find(backup.ID).Logs;
        }
        */

        public bool SaveConfiguration(Client client, Configuration config)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}
