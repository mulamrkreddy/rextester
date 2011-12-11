using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reExp.Controllers.rundotnet;
using reExp.Controllers.regex;
using System.Security.Cryptography;
using reExp.Utils;

namespace reExp.Models
{
    public class Model
    {
        public static void LogCodeData(string data, string result)
        {
            DB.DB.Log_Code_Insert(data, result);
        }

        public static void LogInfo(string info, string type)
        {
            DB.DB.Log_Info_Insert(info, type);
        }
      
        public static string SaveCode(RundotnetData data)
        {
            Random rg = new Random();
            var guid = Utils.Utils.RandomString() + rg.Next(1000, 100000).ToString();
            
            try
            {
                DB.DB.Code_Insert(data.Program, data.SavedOutput, Convert.ToInt32(data.LanguageChoice), Convert.ToInt32(data.EditorChoice), guid, SessionManager.UserId, data.WholeError, data.WholeWarning, data.ShowWarnings, (int)data.Status, data.StatsToSave);
            }
            catch (Exception)
            {
                return "";    
            }
            return guid;
        }

        public static Code GetCode(string guid)
        {
            try
            {
                var res = DB.DB.Code_Get(guid);
                if (res.Count != 0)
                    return new Code()
                    {
                        Program = (string)res[0]["code"],
                        Output = (string)(res[0]["output"] == DBNull.Value ? "" : res[0]["output"]),
                        Lang = Convert.ToInt32(res[0]["lang"]),
                        Editor = Convert.ToInt32(res[0]["editor"]),
                        Guid = (string)res[0]["guid"],
                        WholeError = (string)(res[0]["error"] == DBNull.Value ? "" : res[0]["error"]),
                        Warnings = (string)(res[0]["warning"] == DBNull.Value ? "" : res[0]["warning"]),
                        Status = (res[0]["status"] == DBNull.Value ? GlobalConst.RundotnetStatus.Unknown : Convert.ToInt32(res[0]["status"]).ToRundotnetStatus()),
                        Stats = (string)(res[0]["stats"] == DBNull.Value ? "" : res[0]["stats"]),
                        ShowWarnings = (res[0]["show_warnings"] == DBNull.Value ? false : (bool)res[0]["show_warnings"])
                    };
                else
                    return new Code();
            }
            catch (Exception)
            {
                return new Code();
            }
        }

        public static string SaveRegex(RegexData data)
        {
            Random rg = new Random();
            var guid = Utils.Utils.RandomString() + rg.Next(1000, 100000).ToString();

            try
            {
                string options = "";
                foreach (var o in data.Options)
                    options += o ? "1" : "0";
                DB.DB.Regex_Insert(data.Pattern, data.Text, data.SavedOutput, options, guid, SessionManager.UserId);
            }
            catch (Exception)
            {
                return "";
            }
            return guid;
        }

        public static Regexpr GetRegex(string guid)
        {
            try
            {
                var res = DB.DB.Regex_Get(guid);
                if (res.Count != 0)
                {
                    List<bool> options = new List<bool>();
                    foreach (var ch in (string)res[0]["options"])
                        options.Add(ch == '1' ? true : false);
                    return new Regexpr()
                    {
                        Pattern = (string)res[0]["regex"],
                        Text = (string)res[0]["text"],
                        Output = (string)(res[0]["output"] == DBNull.Value ? "" : res[0]["output"]),
                        Options = options
                    };
                }
                else
                    return new Regexpr();
            }
            catch (Exception)
            {
                return new Regexpr();
            }
        }

        public static string SaveRegexReplace(RegexData data)
        {
            Random rg = new Random();
            var guid = Utils.Utils.RandomString() + rg.Next(1000, 100000).ToString();

            try
            {
                string options = "";
                foreach (var o in data.Options)
                    options += o ? "1" : "0";
                DB.DB.Regex_Replace_Insert(data.Pattern, data.Substitution, data.Text, data.SavedOutput, options, guid, SessionManager.UserId);
            }
            catch (Exception)
            {
                return "";
            }
            return guid;
        }

        public static RegexReplace GetRegexReplace(string guid)
        {
            try
            {
                var res = DB.DB.Regex_Replace_Get(guid);
                if (res.Count != 0)
                {
                    List<bool> options = new List<bool>();
                    foreach (var ch in (string)res[0]["options"])
                        options.Add(ch == '1' ? true : false);
                    return new RegexReplace()
                    {
                        Pattern = (string)res[0]["regex"],
                        Replacement = (string)res[0]["replacement"],
                        Text = (string)res[0]["text"],
                        Output = (string)(res[0]["output"] == DBNull.Value ? "" : res[0]["output"]),
                        Options = options
                    };
                }
                else
                    return new RegexReplace();
            }
            catch (Exception)
            {
                return new RegexReplace();
            }
        }

        public static Registering RegisterUser(string name, string password)
        {
            try
            {
                var res = DB.DB.GetUser(name);
                if (res.Count != 0)
                    return new Registering() { NameTaken = true };

                string hashedPass = Utils.EncryptionUtils.CreateMD5Hash(password);
                DB.DB.InsertUser(name, hashedPass);

                var login = LoginUser(name, password);
                if (login.BadPassword || login.NoSuchUser || !string.IsNullOrEmpty(login.Error))
                    return new Registering() { Error = "Registration successful but login is not." };

                return new Registering();
            }
            catch (Exception e)
            {
                return new Registering() { Error = e.Message };
            }
        }
        public static User LoginUser(string name, string password)
        {
            try
            {
                string hashedPass = Utils.EncryptionUtils.CreateMD5Hash(password);
                var res = DB.DB.GetUser(name);
                if (res.Count == 0)
                    return new User() { NoSuchUser = true };

                if ((string)res[0]["password"] != hashedPass)
                    return new User() { BadPassword = true };

                //set data to session
                SessionManager.UserId = Convert.ToInt32(res[0]["id"]);
                SessionManager.UserName = (string)res[0]["name"];
                return new User() { Name = name };
            }
            catch(Exception e)
            {
                return new User() { Error = e.Message };
            }
        }


        public static List<Code> GetUsersCode(int userId)
        {
            try
            {
                List<Code> userCode = new List<Code>();
                var res = DB.DB.GetUsersCode(userId);
                foreach(var ucode in res)
                {
                    userCode.Add(new Code()
                    {
                        Program = (string)ucode["code"],
                        Lang = Convert.ToInt32(ucode["lang"]),
                        Editor = Convert.ToInt32(ucode["editor"]),
                        Guid = (string)ucode["guid"],
                        Date = (DateTime)ucode["date"],
                        Status = (ucode["status"] == DBNull.Value ? GlobalConst.RundotnetStatus.Unknown : Convert.ToInt32(ucode["status"]).ToRundotnetStatus())
                        //Output = (string)(ucode["output"] == DBNull.Value ? "" : ucode["output"]),
                    });
                }
                return userCode;
            }
            catch (Exception)
            {
                return new List<Code>();
            }
        }


        public static List<Regexpr> GetUsersRegex(int userId)
        {
            try
            {
                List<Regexpr> res = new List<Regexpr>();
                var regexes = DB.DB.GetUsersRegex(userId);
                foreach(var uregex in regexes)
                {
                    List<bool> options = new List<bool>();
                    foreach (var ch in (string)uregex["options"])
                        options.Add(ch == '1' ? true : false);
                    res.Add(new Regexpr()
                    {
                        Pattern = (string)uregex["regex"],
                        Text = (string)uregex["text"],
                        Options = options,
                        Date = (DateTime)uregex["date"],
                        //Output = (string)(uregex["output"] == DBNull.Value ? "" : uregex["output"]),
                        Guid = (string)(uregex["guid"])
                    });
                }
                return res;
            }
            catch (Exception)
            {
                return new List<Regexpr>();
            }
        }


        public static List<RegexReplace> GetUsersRegexReplace(int userId)
        {
            try
            {
                List<RegexReplace> res = new List<RegexReplace>();
                var replaces = DB.DB.GetUsersRegexReplace(userId);
                foreach (var ureplace in replaces)
                { 
                    List<bool> options = new List<bool>();
                    foreach (var ch in (string)ureplace["options"])
                        options.Add(ch == '1' ? true : false);
                    res.Add(new RegexReplace()
                    {
                        Pattern = (string)ureplace["regex"],
                        Replacement = (string)ureplace["replacement"],
                        Text = (string)ureplace["text"],
                        Options = options,
                        Date = (DateTime)ureplace["date"],
                        //Output = (string)(ureplace["output"] == DBNull.Value ? "" : ureplace["output"]),
                        Guid = (string)(ureplace["guid"])
                    });
                }
                return res;               
            }
            catch (Exception)
            {
                return new List<RegexReplace>();
            }
        } 
    }

    public class Registering
    {
        public bool NameTaken
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

    public class User
    {
        public string Name
        {
            get;
            set;
        }
        public bool NoSuchUser
        {
            get;
            set;
        }
        public bool BadPassword
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

    public class Regexpr
    {
        public string Pattern
        {
            get;
            set;
        }
        public string Text
        {
            get;
            set;
        }
        public string Output
        {
            get;
            set;
        }
        public List<bool> Options
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public string Guid
        {
            get;
            set;
        }
    }
    public class RegexReplace
    {
        public string Pattern
        {
            get;
            set;
        }
        public string Replacement
        {
            get;
            set;
        }
        public string Text
        {
            get;
            set;
        }
        public string Output
        {
            get;
            set;
        }
        public List<bool> Options
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public string Guid
        {
            get;
            set;
        }
    }
    public class Code
    {
        public string Program
        {
            get;
            set;
        }
        public string Output
        {
            get;
            set;
        }
        public int Lang
        {
            get;
            set;
        }
        public int Editor
        {
            get;
            set;
        }
        public string Guid
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public string WholeError
        {
            get;
            set;
        }
        public string Warnings
        {
            get;
            set;
        }
        public GlobalConst.RundotnetStatus Status
        {
            get;
            set;
        }
        public string Stats
        {
            get;
            set;
        }
        public bool ShowWarnings
        {
            get;
            set;
        }
    }   
}