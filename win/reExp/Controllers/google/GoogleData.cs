using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reExp.Controllers.google
{
    public class GoogleData
    {
        public string Query { get; set; }
        public string SearchFor { get; set; }
        public bool isResult { get; set; }
        public List<KeyValuePair<int, string>> Result { get; set; }
    }
}