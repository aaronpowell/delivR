using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using SignalR.Hosting;

namespace DelivR.Samples.Basic
{
    public class Basic : FileConnection
    {
        protected override Task OnConnectedAsync(IRequest request, IEnumerable<string> groups, string connectionId)
        {
            this.SendFile(connectionId, Path.Combine(@"C:\_Projects\github\DelivR\DelivR.Samples\Basic", "signalr_18579_lg.gif"));
            return base.OnConnectedAsync(request, groups, connectionId);
        }
    }
}