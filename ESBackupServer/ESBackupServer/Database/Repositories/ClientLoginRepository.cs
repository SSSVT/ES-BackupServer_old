using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Repositories
{
    internal class ClientLoginRepository : AbRepository<ClientLogin>
    {
        internal override List<ClientLogin> FindAll()
        {
            return new List<ClientLogin>();
        }
    }
}