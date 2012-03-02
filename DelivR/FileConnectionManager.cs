using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR;
using SignalR.Infrastructure;
using SignalR.MessageBus;

namespace DelivR
{
    public class FileConnectionManager<T> : Connection
        where T : FileConnection
    {
        public FileConnectionManager(IMessageBus messageBus,
                          IJsonSerializer jsonSerializer,
                          ITraceManager traceManager)
            : base(messageBus, jsonSerializer, typeof(T).FullName, null, new[] { typeof(T).FullName }, Enumerable.Empty<string>(), traceManager)
        {
        }

        public void SendFile(string mimeType, string data)
        {
            this.Broadcast(new
            {
                type = "receive",
                data = new
                {
                    mimeType = mimeType,
                    content = data
                }
            });
        }
    }
}