using System;

namespace ESBackupServer.App.Objects.Authentication
{
    public class LoginResponse
    {
        public Guid SessionID { get; set; }
        public DateTime UTCExpiration { get; set; }
    }
}