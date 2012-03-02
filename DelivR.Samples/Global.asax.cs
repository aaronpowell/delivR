using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using SignalR.Hosting.AspNet.Routing;

namespace DelivR.Samples
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapConnection<Basic.Basic>("basic", "basic/{*operation}");
            RouteTable.Routes.MapConnection<ScreenSharing.ScreenSharing>("screen", "screensharing/{*operation}");
        }
    }
}