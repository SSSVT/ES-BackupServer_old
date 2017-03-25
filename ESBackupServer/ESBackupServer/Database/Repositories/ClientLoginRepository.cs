using ESBackupServer.Database.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Repositories
{
    internal class ClientLoginRepository : AbRepository<Login>
    {
        internal override List<Login> FindAll()
        {
            return new List<Login>();
        }
    }
}