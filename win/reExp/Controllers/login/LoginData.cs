using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reExp.Controllers.login
{
    public class LoginData
    {
        public string Name
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string RegName
        {
            get;
            set;
        }

        public string RegPassword
        {
            get;
            set;
        }

        public bool IsError
        {
            get;
            set;
        }

        public string Error
        {
            get;
            set;
        }

    }
}