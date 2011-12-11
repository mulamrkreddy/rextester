using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reExp.Controllers.feedback
{
    public class Feedback
    {
        public bool IsResult
        {
            get;
            set;
        }
        public bool Succeeded
        {
            get;
            set;
        }
        public string ErrorMessage
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public string InfoString
        {
            get;
            set;
        }
        public string CaptchaRegex
        {
            get;
            set;
        }
        public string UserAnswer
        {
            get;
            set;
        }
    }
}