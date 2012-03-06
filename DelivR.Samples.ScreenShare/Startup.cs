using System;
using System.IO;
using Gate;
using Gate.Middleware;
using Owin;
using SignalR.Hosting.Owin;
using SignalR.Hubs;
using SignalR.Infrastructure;

namespace DelivR.Samples.ScreenShare
{
    public class Startup
    {
        private static IDependencyResolver dependencyResolver;

        public static IDependencyResolver DependencyResolver
        {
            get
            {
                if (dependencyResolver == null)
                {
                    dependencyResolver = new DefaultDependencyResolver();
                }

                return dependencyResolver;
            }
        }

        public static void Configuration(IAppBuilder builder)
        {
            var applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var contentFolder = Path.Combine(applicationBase, "..\\..\\Content");
            var scriptsFolder = Path.Combine(applicationBase, "..\\..\\Scripts");

            builder
                .Use(LogToConsole)
                .UseShowExceptions()
                .UseSignalR("/signalr", DependencyResolver)
                .UseSignalR<ScreenSharing>("/screensharing", DependencyResolver)
                .Use(Alias, "/", "/Screen.html")
                .UseStatic(contentFolder)
                .UseStatic(scriptsFolder)
                ;
        }

        public static AppDelegate Alias(AppDelegate app, string path, string alias)
        {
            return
                (env, result, fault) =>
                {
                    var req = new Request(env);
                    if (req.Path == path)
                    {
                        req.Path = alias;
                    }
                    app(env, result, fault);
                };
        }

        public static AppDelegate LogToConsole(AppDelegate app)
        {
            return
                (env, result, fault) =>
                {
                    var req = new Request(env);
                    Console.WriteLine(req.Method + " " + req.PathBase + req.Path);
                    app(env, result, fault);
                };
        }
    }
}
