using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace reExp.Controllers.google
{
    public class GoogleJob
    {
        public List<KeyValuePair<int, string>> Positions { get; set; }
        public string Error { get; set; }
        string url;
        string searchFor;
        int resultsPerPage;
        public GoogleJob(string url, string searchFor, int resultsPerPage)
        {
            this.url = url;
            this.searchFor = searchFor;
            this.resultsPerPage = resultsPerPage;
            Positions = new List<KeyValuePair<int, string>>();
            Error = null;
        }

        public void DoTheJob()
        {
            try
            {
                string page = PageReader.ReadPage(url, true);

                int resultsStart = page.IndexOf("Search Results", StringComparison.InvariantCulture);
                if (resultsStart != -1)
                    page = page.Substring(resultsStart);

                Regex rex = new Regex(@"<a.*?href=.*?</\s*a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                MatchCollection coll = rex.Matches(page);
                int total = coll.Count;
                if (total > 0)
                {
                    for (int i = 0; i < coll.Count; i++)
                        if (coll[i].Value.Contains(searchFor))
                        {
                            string description = Regex.Replace(coll[i].Value, "(<.*?>)|(;.*?&)|(&.*?;)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            Positions.Add(new KeyValuePair<int, string>(i + 1, description));
                        }

                    double normalizer = 2.5;
                    double newOne = (double)total / (double)resultsPerPage;
                    if (newOne > normalizer)
                        normalizer = newOne;

                    for (int i = 0; i < Positions.Count; i++)
                    {
                        Positions[i] = new KeyValuePair<int, string>((int)(Math.Round((double)Positions[i].Key / normalizer, 0)), Positions[i].Value);
                        if (Positions[i].Key == 0)
                            Positions[i] = new KeyValuePair<int, string>(Positions[i].Key + 1, Positions[i].Value);
                    }
                }
            }
            catch (Exception e)
            {
                this.Error = e.Message;
            }
        }
    }
}