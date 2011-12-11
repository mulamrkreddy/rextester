using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Models;

namespace reExp.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        public string Unknown()
        {
            return "<h2>Something went wrong with our application. We will look into this.</h2>";
        }

        public string NotFound()
        {
            return "<h2>Requested page not found.</h2>";
        }
    }
}
