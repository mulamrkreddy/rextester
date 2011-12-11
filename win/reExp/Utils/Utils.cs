using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Net;
using reExp.Models;
using System.Threading;
using System.Security.Cryptography;
using System.Text;

namespace reExp.Utils
{
    public class Utils
    {
        public static string RootFolder
        {
            get;
            set;
        }

        public static string RandomString()
        {
            Random rg = new Random();
            string res = "";
            for (int i = 0; i < rg.Next(3, 7); i++)
                res += (char)rg.Next((int)'A', (int)'Z'+1);
            return res;
        }

        public static string BaseUrl = @"http://localhost:1115/";
        public static string CurrentPath
        {
            get
            {
                return HttpContext.Current.Request.Url.AbsolutePath;
            }
        }

        public static string GetUrl(PagesEnum page)
        {
            switch (page)
            { 
                case PagesEnum.Home:
                    return BaseUrl;
                case PagesEnum.Tester:
                    return BaseUrl + "tester";
                case PagesEnum.Replace:
                    return BaseUrl + "replace";
                case PagesEnum.Reference:
                    return BaseUrl + "reference";
                case PagesEnum.Pagerank:
                    return BaseUrl + "pagerank";
                case PagesEnum.Rundotnet:
                    return BaseUrl + "runcode";
                case PagesEnum.Feedback:
                    return BaseUrl + "feedback";
                case PagesEnum.UsersStuff:
                    return BaseUrl + "login/usersstuff";
                case PagesEnum.Login:
                    return BaseUrl + "login";
                case PagesEnum.Logout:
                    return BaseUrl + "login/logout";
                default:
                    return BaseUrl;
            }
        }

        public static PagesEnum GetCurrentPage()
        {
            string pagePath = HttpContext.Current.Request.Url.AbsolutePath;
            string[] parts = pagePath.Split(new string[] { @"/" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
            {
                string page = parts[parts.Length - 1].ToLower();

                    if(page.Contains("tester"))
                        return PagesEnum.Tester;
                    if(page.Contains("replace"))
                        return PagesEnum.Replace;
                    if(page.Contains("reference"))
                        return PagesEnum.Reference;
                    if(page.Contains("pagerank"))
                        return PagesEnum.Pagerank;
                    if (page.Contains("rundotnet") || page.Contains("runcode") || new System.Text.RegularExpressions.Regex(@"[a-z]+\d+").IsMatch(page))
                        return PagesEnum.Rundotnet;
                    if (page.Contains("feedback"))
                        return PagesEnum.Feedback;
                    if (page.Contains("usersstuff"))
                        return PagesEnum.UsersStuff;
                    if (page.Contains("login"))
                        return PagesEnum.Login;
                    if (page.Contains("logout"))
                        return PagesEnum.Logout;

                    return PagesEnum.Unknown;
            }
            else
                return PagesEnum.Home;
        }

        public enum PagesEnum
        { 
            Home,
            Tester,
            Replace,
            Reference,
            Pagerank,
            Rundotnet,
            Feedback,
            UsersStuff,
            Login,
            Logout,
            Unknown
        }
    }
    
    public class Log
    {
        public static void LogInfo(string info, string type)
        {
            try
            {               
                LogJob job = new LogJob(info, type);
                System.Threading.Thread worker = new System.Threading.Thread(job.DoWork);
                worker.Start();
            }
            catch(Exception)
            {}
        }

        public static void LogCodeToDB(string data, string result)
        {
            try
            {
                var job = new LogDBJob(data, result);
                System.Threading.Thread worker = new System.Threading.Thread(job.DoWork);
                worker.Start();
            }
            catch (Exception)
            { }
        }
    }

    class LogJob
    {
        string Info
        {
            get;
            set;
        }

        string Type
        {
            get;
            set;
        }

        public LogJob(string info, string type)
        {
            this.Info = info;
            this.Type = type;
        }

        public void DoWork()
        {
            try
            {
                Model.LogInfo(Info, Type);
            }
            catch (Exception)
            { }
            try
            {
               //send mail
            }
            catch (Exception)
            { }
        }
    }

    class LogDBJob
    {
        string data;
        string result;
        public LogDBJob(string data, string result)
        {
            this.data = data;
            this.result = result;
        }
        public void DoWork()
        {
            try
            {
                Model.LogCodeData(data, result);
            }
            catch (Exception)
            { }
        }
    }

    public class CleanUp
    {
        public static void DeleteFile(string path)
        {
            try
            {
                DeleteFileJob job = new DeleteFileJob(path);
                System.Threading.Thread worker = new System.Threading.Thread(job.DoWork);
                worker.Start();
            }
            catch (Exception)
            { }
        }
    }

    class DeleteFileJob
    {
        string Path
        {
            get;
            set;
        }
        public DeleteFileJob(string path)
        {
            this.Path = path;
        }

        public void DoWork()
        {
            System.Threading.Thread.Sleep(5000);
            try
            {
                File.Delete(Path);
            }
            catch (Exception)
            {
            }
        }
    }


   

    class EncryptionUtils
    {
        public static byte[] EncodeDecode(byte[] infoBytes)
        {
            string skey = "test";
            byte[] secretKey = System.Text.Encoding.Unicode.GetBytes(skey);

            if (secretKey.Length > infoBytes.Length)
            {
                List<byte> tmp = new List<byte>();
                for (int i = 0; i < infoBytes.Length; i++)
                    tmp[i] = secretKey[i];
                secretKey = tmp.ToArray();
            }
            else if (secretKey.Length < infoBytes.Length)
            {
                List<byte> tmp = new List<byte>(secretKey);
                for (int i = 0; i < infoBytes.Length - secretKey.Length; i++)
                    tmp.Add(secretKey[i % secretKey.Length]);
                secretKey = tmp.ToArray();
            }

            for (int i = 0; i < secretKey.Length; i++)
                infoBytes[i] = (byte)(infoBytes[i] ^ secretKey[i]);

            return infoBytes;
        }

        public static string ToUserString(byte[] bytes)
        {
            string tmp = "";
            foreach (byte b in bytes)
                tmp += Convert.ToInt16(b).ToString() + "|";
            return tmp;
        }
        public static byte[] FromUserString(string s)
        {
            List<byte> bytes = new List<byte>();
            string[] parts = s.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
                bytes.Add((byte)Convert.ToInt16(part));
            return bytes.ToArray();
        }


        public static string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.Unicode.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    public class SessionManager
    {
        public static int? UserId
        {
            get
            {
                return (int?)HttpContext.Current.Session["userId"];
            }
            set
            {
                HttpContext.Current.Session.Timeout = 20;
                HttpContext.Current.Session["userId"] = value;
            }
        }

        public static string UserName
        {
            get
            {
                return (string)HttpContext.Current.Session["userName"];
            }
            set
            {
                HttpContext.Current.Session["userName"] = value;
            }
        }
        public static bool IsUserInSession()
        {
            return UserId == null ? false : true;
        }
        public static void LogOut()
        {
            try
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }
            catch (Exception)
            { }
        }
    }

    public class GlobalConst
    {
        public enum RundotnetStatus : int
        { 
            Error = 0,
            OK = 1,
            Unknown = 2
        }
    }
}