using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace reExp.Controllers.google
{
    public class GoogleJobs
    {
        public static List<KeyValuePair<int, string>> SpawnSomeJobs(string query, string searchFor)
        {
            List<GoogleJob> jobs = new List<GoogleJob>();
            string baseUrl = "http://www.google.com/search?q={0}&hl=en&authuser=0&num={1}&lr=&ft=i&cr=&safe=images&tbs=";
            string[] parts = query.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string queryInParts = "";
            for(int i=0; i<parts.Length; i++)
            {
                queryInParts+=parts[i];
                if(i < parts.Length-1)
                    queryInParts+="+";
            }
            int resultsPerPage = 100;
            baseUrl = string.Format(baseUrl, queryInParts, resultsPerPage);
            
            jobs.Add(new GoogleJob(baseUrl, searchFor, resultsPerPage));
            int count = resultsPerPage;

            for (int i = 1; i < 10; i++)
            {
                jobs.Add(new GoogleJob(baseUrl + "&start=" + resultsPerPage * i, searchFor, resultsPerPage));
                count += resultsPerPage;
            }

            List<Thread> threads = new List<Thread>();
            foreach (GoogleJob job in jobs)
            {
                var thread = new Thread(job.DoTheJob);
                thread.Start();
                threads.Add(thread);
            }

            int maxWaitSec = 10;
            foreach (Thread thread in threads)
                thread.Join(maxWaitSec * 1000);

            for (int i = 0; i < threads.Count; i++)
                if (threads[i].ThreadState != ThreadState.Stopped)
                {
                    threads[i].Abort();
                    jobs[i].Error = "Thread taking too long. Aborting.";
                }

            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>();
            count = 0;
            foreach (GoogleJob job in jobs)
            {
                if (job.Error != null)
                {
                    result.Add(new KeyValuePair<int, string>(-1, string.Format("Failed to retrieve search results in the range [{0}; {1}]. Reason: '{2}'", count * resultsPerPage, (count+1) * resultsPerPage, job.Error)));
                    count++;
                    continue;
                }
                if (job.Positions.Count != 0)
                    for (int i = 0; i < job.Positions.Count; i++)
                        result.Add(new KeyValuePair<int,string>(count * resultsPerPage + job.Positions[i].Key, job.Positions[i].Value));
                count++;
            }
            return result;
        }
    }
}