using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Routing;

namespace reExp.Views.Shared
{
    /// <summary>
    /// Summary description for KeepSessionAlive
    /// </summary>
    public class KeepSessionAlive : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Session["KeepSessionAlive"] = DateTime.Now;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class KeepSessionAliveRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new KeepSessionAlive();
        }
    }
}