using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;

namespace reExp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add(new Route("{some}/{stuff}.ashx", new reExp.Views.Shared.KeepSessionAliveRouteHandler()));


            routes.MapRoute(
               null,
               "runcode",
               new { controller = "RunDotNet", action = "Index", savedNr = (string)null}
           );

            routes.MapRoute(
               null,
               "{savedNr}",
               new { controller = "RunDotNet", action = "Index" },
               new { savedNr = @"[A-Z]+\d+" }
           );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Main", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

           
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            reExp.Utils.Utils.RootFolder = Server.MapPath("~/");
            if (System.Environment.GetEnvironmentVariable("FSHARP_BIN") != reExp.Utils.Utils.RootFolder + @"bin\4")
                System.Environment.SetEnvironmentVariable("FSHARP_BIN", reExp.Utils.Utils.RootFolder + @"bin\4");

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            HttpException httpException = exception as HttpException;

            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                Response.Redirect("~/Error/NotFound");
            }
            else
            {
                reExp.Utils.Log.LogInfo(DateTime.Now + " \n" + exception.Message + " \n" + exception.StackTrace, "ERROR");
                Response.Redirect("~/Error/Unknown");
            }
        }
    }
}