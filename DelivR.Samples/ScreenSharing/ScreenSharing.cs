﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SignalR.Hosting;
namespace DelivR.Samples.ScreenSharing
{
    public class ScreenSharing : FileConnection
    {
        protected override Task OnConnectedAsync(IRequest request, IEnumerable<string> groups, string connectionId)
        {
            var conn = new SignalR.Client.Connection("http://localhost:8081/screen");

            conn.Received += data =>
                {
                    dynamic deserialized = this._jsonSerializer.Parse(data);
                    this.SendFile(connectionId, (string)deserialized.data.mimeType, (string)deserialized.data.content);
                };

            conn.Start().Wait();

            return base.OnConnectedAsync(request, groups, connectionId);
        }
    }
}