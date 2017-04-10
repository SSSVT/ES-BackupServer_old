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
        All,
        [EnumMember]
        Verified,
        [EnumMember]
        Unverified,
        [EnumMember]
        Banned
    }

    public enum Sort
    {
        [EnumMember]
        Asc,
        [EnumMember]
        Desc
    }
}