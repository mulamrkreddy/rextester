using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Models;
namespace reExp.Controllers.login
{
    public class UserData
    {
        public string UserName
        {
            get;
            set;
        }
        public List<Code> Snippets
        {
            get;
            set;
        }

        public List<Regexpr> Regexes
        {
            get;
            set;
        }

        public List<RegexReplace> Replaces
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