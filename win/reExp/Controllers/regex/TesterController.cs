using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Models;
using reExp.Utils;

namespace reExp.Controllers.regex
{
    public class TesterController : Controller
    {
        //
        // GET: /Tester/

        [ValidateInput(false)]
        public ActionResult Index(RegexData data)
        {
            Compression.SetCompression();
            data.Describtions = (new OptionDescribtion()).GetDescribtions();
            data.IsReplace = false;

            //retrieve saved regex
            if (!string.IsNullOrEmpty(HttpContext.Request.QueryString["code"]))
            {
                var regex = Model.GetRegex(HttpContext.Request["code"]);
                data.Pattern = regex.Pattern;
                data.Text = regex.Text;
                if (regex.Options != null)
                    data.Options = regex.Options;
                data.Result = regex.Output;
                data.IsResult = true;
                return View(data);
            }
            //save regex
            if (data.ShouldSave)
            {
                if (string.IsNullOrEmpty(data.Pattern))
                    data.Pattern = string.Empty;
                if (string.IsNullOrEmpty(data.Text))
                    data.Text = string.Empty;
                string guid = Model.SaveRegex(data);
                if (!string.IsNullOrEmpty(guid))
                    data.SavedUrl = Utils.Utils.GetUrl(Utils.Utils.PagesEnum.Tester) + "?code=" + guid;
                else
                    data.SavedUrl = "";
                data.SavedOutput = "";
                data.ShouldSave = false;
                return View(data);
            }                        
            
            if (string.IsNullOrEmpty(data.Pattern) && string.IsNullOrEmpty(data.Text))
            {
                data.IsResult = false;
                return View(data);
            }

            data.IsResult = true;
            data.Result = Logic.TakeText(data);
            return View(data);
        }

    }
}
