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
using Firefly.Http;
using Gate.Builder;
using Owin;
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
            var builder = new AppBuilder();
            var app = builder.Build(Startup.Configuration);

            var server = new ServerFactory().Create(app, 8081);

            ThreadPool.QueueUserWorkItem(_ =>
                {
                    while (true)
                    {
                        Thread.Sleep(5000);

                        var sc = new ScreenCapture();

                        var image = sc.CaptureScreen();

                        using (var ms = new MemoryStream())
                        {
                            image.Save(ms, ImageFormat.Png);

                            var s = Convert.ToBase64String(ms.ToArray());

                            var connection = new FileConnectionManager<ScreenSharing>(
                                Startup.DependencyResolver.Resolve<IMessageBus>(),
                                Startup.DependencyResolver.Resolve<IJsonSerializer>(),
                                Startup.DependencyResolver.Resolve<ITraceManager>()
                                );

                            connection.SendFile("image/png", s);
                        }

                        Console.WriteLine("Data sent");
                    }
                });

            Console.WriteLine("Running on localhost:8081");

            Console.ReadKey();
        }
    }
}
