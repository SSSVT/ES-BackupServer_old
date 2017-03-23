using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Repositories
{
    internal abstract class AbRepository<T>
    {
        internal abstract List<T> FindAll();

    }
}