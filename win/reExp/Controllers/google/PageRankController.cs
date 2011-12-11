using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace reExp.Controllers.google
{
    public class PageRankController : Controller
    {
        //
        // GET: /PageRank/
        [ValidateInput(false)]
        public ActionResult Index(GoogleData data)
        {
            if (!string.IsNullOrEmpty(data.Query) && !string.IsNullOrEmpty(data.SearchFor))
            {
                data.isResult = true;
                data.Result = GoogleJobs.SpawnSomeJobs(data.Query, data.SearchFor);
            }
            else
                data.isResult = false;
            return View(data);
        }

    }
}
