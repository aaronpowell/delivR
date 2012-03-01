using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DelivR.Samples.Basic
{
    public class Basic : FileConnection
    {
        protected override System.Threading.Tasks.Task OnConnectedAsync(SignalR.Hosting.IRequest request, IEnumerable<string> groups, string connectionId)
        {
            this.SendImage(connectionId, Path.Combine(@"C:\_Projects\github\DelivR\DelivR.Samples\Basic", "signalr_18579_lg.gif"));
            return base.OnConnectedAsync(request, groups, connectionId);
        }
    }
}