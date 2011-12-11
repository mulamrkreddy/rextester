using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Models;
using reExp.Utils;
using System.Web.Script.Serialization;

namespace reExp.Controllers.rundotnet
{
    class JsonData
    {
        public string Url
        {
            get;
            set;
        }
        public string Warnings
        {
            get;
            set;
        }
        public string Errors
        {
            get;
            set;
        }
        public string Result
        {
            get;
            set;
        }
        public string Stats
        {
            get;
            set;
        }
    }

    public class RunDotNetController : Controller
    {
        [ValidateInput(false)]
        public ActionResult Index(RundotnetData data, string savedNr = null)
        {
            Compression.SetCompression();

            //retrieve saved code
            savedNr = savedNr ?? HttpContext.Request.QueryString["code"];
            if (!string.IsNullOrEmpty(savedNr))
            {
                var code = Model.GetCode(savedNr);
                data.Program = code.Program;
                data.LanguageChoice = code.Lang.ToString() == "0" ? "1" : code.Lang.ToString();
                data.EditorChoice = code.Editor.ToString() == "0" ? "1" : code.Editor.ToString();
                data.Output = code.Output;
                data.WholeError = code.WholeError;
                data.WholeWarning = code.Warnings;
                data.ShowWarnings = code.ShowWarnings;
                data.RunStats = code.Stats;
                return View(data);
            }

            if (string.IsNullOrEmpty(data.LanguageChoice))
                data.LanguageChoice = "1";
            if (string.IsNullOrEmpty(data.EditorChoice))
                data.EditorChoice = "1";
            if (data.EditorChoice == "1" && data.ShowCodeMirror == false)
                data.EditorChoice = "2";

            data.Program = data.GetInitialCode(data.LanguageChoice, data.EditorChoice);

            return View(data);
        }

        

        [HttpPost]
        [ValidateInput(false)]
        public string Save(RundotnetData data)
        {
            Compression.SetCompression();

            string url = null;
            if (data.Program == null)
                data.Program = string.Empty;
            if (!string.IsNullOrEmpty(data.WholeError))
                data.Status = GlobalConst.RundotnetStatus.Error;
            else
                data.Status = GlobalConst.RundotnetStatus.OK;
            string guid = Model.SaveCode(data);
            if (!string.IsNullOrEmpty(guid))
                url = Utils.Utils.BaseUrl + guid;

            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(new JsonData() { Url = url });
        }

        [HttpPost]
        [ValidateInput(false)]
        public string Run(RundotnetData data)
        {
            Compression.SetCompression();
            JavaScriptSerializer json = new JavaScriptSerializer();            

            if (!string.IsNullOrEmpty(data.Program) && data.Program.Length > 200000)
            {
                return json.Serialize(new JsonData() { Errors = "Program is too long (max is 200000 characters).\n" });
            }

            data.Warnings = new List<string>();
            data.Errors = new List<string>();
            data = RundotnetLogic.RunProgram(data);
            string warnings = null, errors = null;
            if(data.Warnings.Count() != 0 && data.ShowWarnings)
                warnings = data.Warnings.Aggregate((a, b) => a + "\n" + b);
            if (data.Errors.Count() != 0)
                errors = data.Errors.Aggregate((a, b) => a + "\n" + b);
            return json.Serialize(new JsonData() { Warnings = warnings, Errors = errors, Result = data.Output, Stats = data.RunStats});
        }
    }
}
