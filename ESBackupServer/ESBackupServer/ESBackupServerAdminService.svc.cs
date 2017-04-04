using ESBackupServer.App.Objects;
using ESBackupServer.App.Objects.Factories;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System.Collections.Generic;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ESBackupServerAdminService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ESBackupServerAdminService.svc or ESBackupServerAdminService.svc.cs at the Solution Explorer and start debugging.
    public class ESBackupServerAdminService : IESBackupServerAdminService
    {
        //TODO: Optional - Fix bug - metadata obtaining (https://msdn.microsoft.com/en-us/library/aa751951.aspx)
        //TODO: Optional - Implement https://www.codeproject.com/Articles/763271/Common-issues-in-WCF

        #region Properties
        private ClientRepository _ClientRepository { get; set; } = ClientRepository.GetInstance();
        private LogRepository _LogRepository { get; set; } = LogRepository.GetInstance();

        private ConfigurationFactory _ConfigFactory { get; set; } = new ConfigurationFactory();
        //TODO: Check
        private BackupRepository _BackupRepository { get; set; } = BackupRepository.GetInstance();
        #endregion

        #region Get
        //public List<Backup> GetBackups(Client client)
        //{
        //    return this._ClientRepository.Find(client.ID).Backups;
        //}
        //TODO: Check
        public List<Backup> GetBackups(Client client)
        {
            return this._BackupRepository.Find(client);
        }
        public List<Client> GetClients()
        {
            return this._ClientRepository.FindAll();
        }
        public Configuration GetConfiguration(Client client)
        {
            return this._ConfigFactory.Create(client);
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
        public bool SaveConfiguration(Configuration config)
        {
            return this._ConfigFactory.Save(config);
        }
        #endregion
    }
}
