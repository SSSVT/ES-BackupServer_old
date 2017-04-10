using ESBackupServer.App.Objects;
using ESBackupServer.App.Objects.Factories;
using ESBackupServer.App.Objects.Filters;
using ESBackupServer.Database.Objects;
using ESBackupServer.Database.Repositories;
using System;
using System.Collections.Generic;

namespace ESBackupServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ESBackupServerAdminService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ESBackupServerAdminService.svc or ESBackupServerAdminService.svc.cs at the Solution Explorer and start debugging.
    public class ESBackupServerAdminService : IESBackupServerAdminService
    {
        #region Properties
        #region Repositories
        private ClientRepository _ClientRepository { get; set; } = ClientRepository.GetInstance();
        private LogRepository _LogRepository { get; set; } = LogRepository.GetInstance();
        private BackupRepository _BackupRepository { get; set; } = BackupRepository.GetInstance();
        #endregion

        private ConfigurationFactory _ConfigFactory { get; set; } = new ConfigurationFactory();
        #endregion

        #region Get
        public List<Backup> GetBackupsByClientID(int id)
        {
            return this._BackupRepository.FindByClientID(id);
        }
        public Backup GetBackupByID(long id)
        {
            return this._BackupRepository.Find(id);
        }
        public List<Client> GetClients(Filter filter, Sort sort)
        {
            return this._ClientRepository.Find(filter, sort);
        }
        public Configuration GetConfiguration(Client client)
        {
            return this._ConfigFactory.Create(client);
        }        
        public List<Log> GetLogsByClientID(int id)
        {
            return this._LogRepository.FindByClientID(id);
        }        
        public List<Log> GetLogsByBackupID(long id)
        {
            return this._LogRepository.FindByBackupID(id);
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
 