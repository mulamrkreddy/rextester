using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Models;
using System.Text.RegularExpressions;
using System.Threading;
using System.Text;
using reExp.Utils;
using reExp.Controllers.regex;

namespace reExp.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/ 
        public ActionResult Index()
        {
            Compression.SetCompression();
            return View();
        }

        [ValidateInput(false)]
        public string TakeText(RegexData data)
        {
            string res = Logic.TakeText(data);
            if(res.Length >= Compression.MaxUncompressedLength)
                Compression.SetCompression();
            return res;
        }
    }
}
