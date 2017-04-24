using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESBackupServer.Database.Objects
{
    public class Administrator
    {
        //TODO: Implement
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}