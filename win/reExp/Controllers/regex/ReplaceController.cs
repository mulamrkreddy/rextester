using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Models;
using reExp.Utils;

namespace reExp.Controllers.regex
{
    public class ReplaceController : Controller
    {
        //
        // GET: /Replace/
        [ValidateInput(false)]
        public ActionResult Index(RegexData data)
        {
            Compression.SetCompression();
            data.IsReplace = true;
            data.Describtions = (new OptionDescribtion()).GetDescribtions();

            //retrieve saved regex replace 
            if (!string.IsNullOrEmpty(HttpContext.Request.QueryString["code"]))
            {
                var regexReplace = Model.GetRegexReplace(HttpContext.Request["code"]);
                data.Pattern = regexReplace.Pattern;
                data.Substitution = regexReplace.Replacement;
                data.Text = regexReplace.Text;
                if (regexReplace.Options != null)
                    data.Options = regexReplace.Options;
                data.Result = regexReplace.Output;
                data.IsResult = true;
                return View(data);
            }

            //save regex replace
            if (data.ShouldSave)
            {
                if (string.IsNullOrEmpty(data.Pattern))
                    data.Pattern = string.Empty;
                if (string.IsNullOrEmpty(data.Substitution))
                    data.Substitution = string.Empty;
                if (string.IsNullOrEmpty(data.Text))
                    data.Text = string.Empty;
                string guid = Model.SaveRegexReplace(data);
                if (!string.IsNullOrEmpty(guid))
                    data.SavedUrl = Utils.Utils.GetUrl(Utils.Utils.PagesEnum.Replace) + "?code=" + guid;
                else
                    data.SavedUrl = "";
                data.SavedOutput = "";
                data.ShouldSave = false;
                return View(data);
            }

            if (string.IsNullOrEmpty(data.Pattern) && string.IsNullOrEmpty(data.Substitution) && string.IsNullOrEmpty(data.Text))
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
