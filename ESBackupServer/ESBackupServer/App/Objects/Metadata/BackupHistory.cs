using System;

namespace ESBackupServer.App.Objects.Metadata
{
    public class BackupHistory
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime UTCStart { get; set; }
        public DateTime UTCEnd { get; set; }
    }
}