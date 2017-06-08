using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESBackupServer.App.Interfaces.CRON
{
    internal interface ITaskScheduler
    {
        void Run();
        void Stop();
    }
}
