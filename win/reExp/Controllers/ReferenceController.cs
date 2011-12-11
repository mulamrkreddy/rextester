using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Utils;

namespace reExp.Controllers
{
    public class ReferenceController : Controller
    {
        //
        // GET: /Reference/

        public ActionResult Index()
        {
            Compression.SetCompression();
            return View();
        }

    }
}
