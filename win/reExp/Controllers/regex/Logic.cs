using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Models;
using System.Threading;
using System.Text.RegularExpressions;
using System.Text;
using System.Web.Mvc;
using reExp.Utils;

namespace reExp.Controllers.regex
{
    public class Logic
    {
        public static string TakeText(RegexData data)
        {
            try
            {
                if (data.Pattern == null)
                    data.Pattern = "";
                if (!string.IsNullOrEmpty(data.Text) && data.Text.Contains("\r"))
                    data.Text = data.Text.Replace("\r", "");

                int maxLength = 500000;
                if (!string.IsNullOrEmpty(data.Text) && data.Text.Length > maxLength)
                    return string.Format("Text is too long (max {0} characters).", maxLength);
                if (!string.IsNullOrEmpty(data.Pattern) && data.Pattern.Length > maxLength)
                    return string.Format("Pattern is too long (max {0} characters).", maxLength);
                if (!string.IsNullOrEmpty(data.Substitution) && data.Substitution.Length > maxLength)
                    return string.Format("Substitution is too long (max {0} characters).", maxLength);

                Job job = new Job(data);
                Thread worker = new Thread(job.DoWork);
                worker.Start();
                int secToWait = 20;
                worker.Join(secToWait * 1000);
                if (worker.ThreadState != ThreadState.Stopped)
                {
                    worker.Abort();
                    return string.Format("The computation taking too long ( > {0} sec). Giving up.", secToWait);
                }
                else
                    return job.Result;
            }
            catch (Exception e)
            {
                reExp.Utils.Log.LogInfo(e.Message+" \n"+e.StackTrace +" \n", "ERROR_IN_LOGIC");
                return "Error occurred. We'll examine why.";
            }
        }
    }
    class Job
    {
        public string Result
        {
            get;
            set;
        }
        RegexData data;
        public Job(RegexData data)
        {
            this.data = data;
        }


        public void DoWork()
        {
            try
            {
                string attach = "";
                if (!string.IsNullOrEmpty(data.Pattern) && data.Pattern.Contains(@"\r"))
                {
                    var candidates = (new Regex(@"\\+r")).Matches(data.Pattern);
                    foreach (var cand in candidates)
                        if ((cand as Match).Value.Count(f => f == '\\') % 2 != 0)
                        {
                            attach = @"<b>\r (carriage return) is removed from text. Use \n (new line) instead.</b><br/>";
                            break;
                        }
                }
                RegexOptions? option = null;
                var desc = (new OptionDescribtion()).GetDescribtions();
                for (int i = 0; i < data.Options.Count; i++)
                    if (data.Options[i])
                    {
                        if (option == null)
                            option = desc[i].RegexOption;
                        else
                            option = option | desc[i].RegexOption;
                    }

                if (!data.IsReplace)
                    Match(attach, option);
                else
                {
                    if (data.Substitution == null)
                        data.Substitution = "";
                    Replace(attach, option);
                }

                return;
            }
            catch (Exception e)
            {
                reExp.Utils.Log.LogInfo(e.Message + " \n" + e.StackTrace, "ERROR_IN_LOGIC");
                Result = "Error occurred. We'll examine why.";
                return;
            }
        }

        private void Replace(string attach, RegexOptions? option)
        {
            ExecutionStopwatch executionSW = new ExecutionStopwatch();
            executionSW.Start();
            Regex r;
            try
            {
                if (option == null)
                    r = new Regex(data.Pattern);
                else
                    r = new Regex(data.Pattern, (RegexOptions)option);
            }
            catch (Exception e)
            {
                Result = e.Message;
                return;
            }
            if (string.IsNullOrEmpty(data.Text))
            {
                Result = attach + "Pattern ok.";               
                return;
            }
            MatchCollection coll = r.Matches(data.Text);
            int matchesNr = coll.Count;

            string attachStats = "";
            if (matchesNr == 0)
            {
                executionSW.Stop();
                attachStats = string.Format(" ({0} ms)", executionSW.Elapsed.TotalMilliseconds);
                Result = attach + "No matches found." + attachStats;
                return;
            }

            StringBuilder sb = new StringBuilder(attach);
            StringBuilder tb = new StringBuilder();

            var utility = new HtmlHelper(new ViewContext(), new ViewPage());
            List<int> matchesCoords = new List<int>();
            List<string> replacements = new List<string>();
            int count = 0;
            int maxCount = 100;
            string matchedBefore = "<span class=\"magenta\">";
            string matchedAfter = "</span>";
            foreach (Match match in coll)
            {
                int index = match.Index;
                string value = match.Value;
                string replacement = match.Result(data.Substitution);
                matchesCoords.Add(index);
                matchesCoords.Add(value.Length);                
                replacements.Add(replacement);
                if (count < maxCount)
                    sb.Append(string.Format("'"+matchedBefore+"<b>{0}</b>"+matchedAfter+"' replaced by '"+matchedBefore+"<b>{1}</b>"+matchedAfter+"' at {2} <br/>", utility.Encode(value), utility.Encode(replacement), index));                    
                count++;
            }
            executionSW.Stop();
            attachStats = string.Format(" ({0} ms)", executionSW.Elapsed.TotalMilliseconds);
            if(count <= maxCount)
                sb.Append(string.Format("Total replacements: {0}.{1}", matchesNr, attachStats));
            else
                sb.Append(string.Format("other replacements ({0}) not included. Total matches: {1}.{2}", matchesNr - maxCount, matchesNr, attachStats));

            bool IsReverse = (option & RegexOptions.RightToLeft) != null && (option & RegexOptions.RightToLeft) != RegexOptions.None;
            if (IsReverse)
            {
                List<int> tmpMatchesCoords = new List<int>();
                List<string> tmpReplacements = new List<string>();
                for (int i = matchesCoords.Count - 1; i > 0; i -= 2)
                {
                    tmpMatchesCoords.Add(matchesCoords[i - 1]);
                    tmpMatchesCoords.Add(matchesCoords[i]);
                    tmpReplacements.Add(replacements[i / 2]);
                }
                matchesCoords = tmpMatchesCoords;
                replacements = tmpReplacements;
            }
            
            if (matchesNr != 0)
            {
                string before0 = "<span class=\"yellow\">";
                string after = "</span>";
                string before1 = "<span class=\"blue\">";
                bool flipper = true;
                int lastIndex = 0;
                for (int i = 0; i < matchesCoords.Count; i += 2)
                {
                    string before = flipper ? before0 : before1;
                    flipper = !flipper;
                    if (lastIndex != matchesCoords[i])
                        tb.Append(utility.Encode(data.Text.Substring(lastIndex, matchesCoords[i] - lastIndex)));
                    tb.Append(before + utility.Encode(replacements[i / 2]) + after);
                    lastIndex = matchesCoords[i] + matchesCoords[i + 1];
                }
                tb.Append(utility.Encode(data.Text.Substring(lastIndex)));
            }
            else
                tb.Append(utility.Encode(data.Text));

            Result = sb.ToString() + "<br/><br/><br/><span id=\"ResultText\">" + tb.ToString().Replace("\r", "").Replace("\n", "<br/>") + "</span>";
        }

        private void Match(string attach, RegexOptions? option)
        {
            ExecutionStopwatch executionSW = new ExecutionStopwatch();
            executionSW.Start();
            Regex r;
            try
            {
                if (option == null)
                    r = new Regex(data.Pattern);
                else
                    r = new Regex(data.Pattern, (RegexOptions)option);
            }
            catch (Exception e)
            {
                Result = e.Message;
                return;
            }
            if (string.IsNullOrEmpty(data.Text))
            {
                Result = attach + "Pattern ok.";                
                return;
            }
            MatchCollection coll = r.Matches(data.Text);
            int matchesNr = coll.Count;
            executionSW.Stop();
            string attachStats = string.Format(" ({0} ms)", executionSW.Elapsed.TotalMilliseconds);
            if (matchesNr == 0)
            {
                Result = attach + "No matches found." + attachStats;
                return;
            }

            StringBuilder sb = new StringBuilder(attach);
            StringBuilder tb = new StringBuilder();

            var utility = new HtmlHelper(new ViewContext(), new ViewPage());

            int count = 0;
            int maxCount = 100;
            bool addMoreMatches = true;
            List<int> matchIndexes = new List<int>();

            List<Match> collsTmp = new List<Match>();
            List<Match> colls = collsTmp;
            foreach (Match m in coll)
                collsTmp.Add(m);
            bool IsReverse = (option & RegexOptions.RightToLeft) != null && (option & RegexOptions.RightToLeft) != RegexOptions.None;
            if (IsReverse)
            {
                colls = new List<Match>();
                for (int i = collsTmp.Count - 1; i >= 0; i--)
                    colls.Add(collsTmp[i]);
            }
            List<string> reverseMatches = new List<string>();

            foreach (Match match in colls)
            {
                string value = match.Value;
                int index = match.Index;

                matchIndexes.Add(index);
                matchIndexes.Add(value.Length);

                string matchedBefore = "<span class=\"magenta\">";
                string matchedAfter = "</span>";
                count++;
                if (!IsReverse)
                {
                    if (count == maxCount + 1)
                    {
                        sb.Append(string.Format("other matches ({0}) not included. Total matches: {1}.{2}", matchesNr - maxCount, matchesNr, attachStats));
                        addMoreMatches = false;
                    }
                    if (addMoreMatches)
                        sb.Append(string.Format("'"+matchedBefore+"<b>{0}</b>"+matchedAfter+"' found at {1} <br/>", utility.Encode(value), index));
                }
                else
                {
                    if (matchesNr - count < maxCount)
                        reverseMatches.Add(string.Format("'"+matchedBefore+"<b>{0}</b>"+matchedAfter+"' found at {1} <br/>", utility.Encode(value), index));
                }
            }
            if (IsReverse)
            {
                for (int i = reverseMatches.Count - 1; i >= 0; i--)
                    sb.Append(reverseMatches[i]);
                if (matchesNr > maxCount)
                    sb.Append(string.Format("other matches ({0}) not included. Total matches: {1}.{2}", matchesNr - maxCount, matchesNr, attachStats));
                else
                    sb.Append(string.Format("Total matches: {0}.{1}", matchesNr, attachStats));
            }
            else if (addMoreMatches)
                sb.Append(string.Format("Total matches: {0}.{1}", matchesNr, attachStats));


           

            if (matchesNr != 0)
            {
                string before0 = "<span class=\"yellow\">";
                string after = "</span>";
                string before1 = "<span class=\"blue\">";
                bool flipper = true;
                int lastIndex = 0;
                for (int i = 0; i < matchIndexes.Count; i += 2)
                {
                    string before = flipper ? before0 : before1;
                    flipper = !flipper;
                    if (lastIndex != matchIndexes[i])
                        tb.Append(utility.Encode(data.Text.Substring(lastIndex, matchIndexes[i] - lastIndex)));
                    tb.Append(before + utility.Encode(data.Text.Substring(matchIndexes[i], matchIndexes[i + 1])) + after);
                    lastIndex = matchIndexes[i] + matchIndexes[i + 1];
                }
                tb.Append(utility.Encode(data.Text.Substring(lastIndex)));
            }
            else
                tb.Append(utility.Encode(data.Text));

            Result = sb.ToString() + "<br/><br/><br/>" + tb.ToString();//.Replace("\n", "<br/>");        
        }
    }
}