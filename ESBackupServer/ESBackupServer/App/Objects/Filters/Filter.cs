using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ESBackupServer.App.Objects.Filters
{
    [DataContract]
    public enum Filter
    {
        [EnumMember]
        Verified,
        [EnumMember]
        Unverified,
        [EnumMember]
        Banned,
        [EnumMember]
        All
    }

    public enum Sort
    {
        [EnumMember]
        Asc,
        [EnumMember]
        Desc
    }
}