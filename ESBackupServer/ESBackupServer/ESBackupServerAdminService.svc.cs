using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ESBackupServer.App.Objects;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using ESBackupServer.App.Objects.Factories;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ESBackupServerAdminService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ESBackupServerAdminService.svc or ESBackupServerAdminService.svc.cs at the Solution Explorer and start debugging.
    public class ESBackupServerAdminService : IESBackupServerAdminService
    {
        //TODO: Optional - Fix bug - metadata obtaining (https://msdn.microsoft.com/en-us/library/aa751951.aspx)
        //TODO: Optional - Implement https://www.codeproject.com/Articles/763271/Common-issues-in-WCF

        #region Properties
        private BackupRepository _BackupRepository { get; set; } = BackupRepository.GetInstance();
        private ClientRepository _ClientRepository { get; set; } = ClientRepository.GetInstance();
        private LogRepository _LogRepository { get; set; } = LogRepository.GetInstance();
        #endregion

        #region Get
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
            return new ConfigurationFactory().Create(client);
        }
        
        public List<Log> GetLogsByClient(Client client)
        {
            return this._ClientRepository.Find(client.ID).Logs;
        }
        
        public List<Log> GetLogsByBackup(Backup backup)
        {
            return this._LogRepository.Find(backup);
        }
        #endregion
        #region Set
        public bool SaveConfiguration(Client client, Configuration config)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
        #endregion
    }
}
