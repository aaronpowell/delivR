using System.Collections.Generic;
using System.Threading.Tasks;
using SignalR.Hosting;
namespace DelivR.Samples.ScreenSharing
{
    public class ScreenSharing : FileConnection
    {
        private SignalR.Client.Connection conn;

        protected override Task OnConnectedAsync(IRequest request, IEnumerable<string> groups, string connectionId)
        {
            conn = new SignalR.Client.Connection("http://localhost:8081/screen");

            conn.Received += data =>
                {
                    dynamic deserialized = this._jsonSerializer.Parse(data);
                    this.SendRawFile(connectionId, (string)deserialized.data.mimeType, (string)deserialized.data.content);
                };

            conn.Closed += () =>
            {
                this.Send(new {
                    type = "death"
                });
            };

            conn.Reconnected += () =>
            {
                this.Send(new { type = "reconnected" });
            };

            conn.Start();

            return base.OnConnectedAsync(request, groups, connectionId);
        }

    }
}