using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SignalR;
using SignalR.Hosting.Self;
using SignalR.Infrastructure;
using SignalR.MessageBus;

namespace DelivR.Samples.ScreenShare
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8081/";
            var server = new Server(url);
            server.MapConnection<ScreenSharing>("/screen");

            server.Start();

            ThreadPool.QueueUserWorkItem(_ =>
                {
                    while (true)
                    {
                        Thread.Sleep(1000);

                        var sc = new ScreenCapture();

                        var image = sc.CaptureScreen();

                        using (var ms = new MemoryStream())
                        {
                            image.Save(ms, ImageFormat.Png);

                            var s = Convert.ToBase64String(ms.ToArray());

                            var connection = new FileConnectionManager<ScreenSharing>(
                                server.DependencyResolver.Resolve<IMessageBus>(),
                                server.DependencyResolver.Resolve<IJsonSerializer>(),
                                server.DependencyResolver.Resolve<ITraceManager>()
                                );

                            connection.SendFile("image/png", s);
                        }

                        Console.WriteLine("Data sent");
                    }
                });

            Console.WriteLine("Server running on {0}", url);
            Console.ReadKey();
        }
    }
}
