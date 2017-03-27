using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;

namespace ESBackupServer.App.Objects.Components.Net
{
    internal class NetInfoObtainer
    {
        internal IPAddress GetClientIP()
        {
            return IPAddress.Parse(((RemoteEndpointMessageProperty)OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name]).Address);
        }
        internal int GetClientPort()
        {
            return ((RemoteEndpointMessageProperty)OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name]).Port;
        }
    }
}