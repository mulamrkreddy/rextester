using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Models;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Mail;
using System.Net;
using reExp.Utils;

namespace reExp.Controllers.feedback
{
    public class FeedbackController : Controller
    {
        //private static object _lock = new object();

        // GET: /Feedback/
        [ValidateInput(false)]
        public ActionResult Index(Feedback feedback)
        {
            try
            {
                if (string.IsNullOrEmpty(feedback.Message) && string.IsNullOrEmpty(feedback.UserAnswer))
                {
                    feedback.IsResult = false;
                    return View(feedback);
                }
                int maxLength = 30000;
                feedback.IsResult = true;
                
                if (string.IsNullOrEmpty(feedback.Message))
                {
                    feedback.Succeeded = false;
                    feedback.ErrorMessage = "Feedback shouldn't be empty.";
                    return View(feedback);
                }
                else if (feedback.Message.Length > maxLength)
                {
                    feedback.Succeeded = false;
                    feedback.ErrorMessage = string.Format("Feedback shouldn't be longer than {0} characters.", maxLength);
                    return View(feedback);
                }
               
                Log.LogInfo(feedback.Message, "FEEDBACK");
                feedback.Succeeded = true;
                return View(feedback);
            }
            catch (Exception e)
            {
                Log.LogInfo(e.Message, "FEEDBACK_ERROR");
                feedback.Succeeded = false;
                feedback.ErrorMessage = "Oops. Something went wrong by our fault. Please try again.";
                return View(feedback);
            }
        }
    }
}
