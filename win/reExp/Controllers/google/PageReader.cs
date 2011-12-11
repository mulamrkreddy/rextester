using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.IO.Compression;

namespace reExp.Controllers
{
    public class PageReader
    {
        public static string ReadPage(string url, bool useGzip)
        {
            HttpWebResponse response = null;
            Stream resStream = null;

            try
            {
                // used to build entire input
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                // prepare the web page we will be asking for
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                if (useGzip)
                {
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    request.Accept = "image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; FDM; .NET CLR 2.0.50727; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET4.0C; .NET4.0E; AskTB5.6)";
                }
                // Set some reasonable limits on resources used by this request
                request.MaximumAutomaticRedirections = 2;
                request.MaximumResponseHeadersLength = 4;
                request.Timeout = 7000;

                // execute the request
                response = (HttpWebResponse)request.GetResponse();

                // we will read data via the response stream
                //if (useGzip)
                //    resStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                //else
                    resStream = response.GetResponseStream();

                Encoding encoding;
                if (String.IsNullOrEmpty(response.CharacterSet))
                    encoding = Encoding.UTF8;
                else
                    encoding = Encoding.GetEncoding(response.CharacterSet);


                //int maxChars = 50000;
                //byte[] buffer = new byte[maxChars];
                //int count = resStream.Read(buffer, 0, buffer.Length);
                //string res = Encoding.ASCII.GetString(buffer, 0, count);

                string tempString = null;
                int count = 0;
                int maxReads = 20;
                do
                {
                    // fill the buffer with data
                    count = resStream.Read(buf, 0, buf.Length);

                    // make sure we read some data
                    if (count != 0)
                    {
                        // translate from bytes to ASCII text

                        tempString = encoding.GetString(buf, 0, count);
                        //tempString = Encoding.UTF8.GetString(buf, 0, count);

                        // continue building the string
                        sb.Append(tempString);
                    }
                    maxReads--;
                }
                while (count > 0 && maxReads > 0); // any more data to read?



                response.Close();
                resStream.Close();

                return sb.ToString();

            }
            catch (Exception)
            {
                if (response != null)
                    response.Close();
                if (resStream != null)
                    resStream.Close();
                throw;
            }

        }
    }
}