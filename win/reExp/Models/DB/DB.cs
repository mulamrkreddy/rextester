using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using reExp.Utils;


namespace reExp.Models.DB
{
    public class DB
    {
        static string location = Utils.Utils.RootFolder + @"db\db.s3db";
        static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(string.Format("Data Source={0};Version=3;", location));
        }

        public static List<Dictionary<string, object>> GetUser(string name)
        {
            string query = @"select * from Users where name = @Name";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Name", name));
            return ExecuteQuery(query, pars);
        }

        public static void InsertUser(string name, string passwordHash)
        {
            string query = @"insert into Users(name, password) values(@Name, @Password)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Name", name));
            pars.Add(new SQLiteParameter("Password", passwordHash));
            ExecuteNonQuery(query, pars);
        }

        public static void Log_Code_Insert(string data, string result)
        {
            string query = @"insert into LogCode(data, result) values(@Data, @Result)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Data", data));
            pars.Add(new SQLiteParameter("Result", result));
            ExecuteNonQuery(query, pars);
        }

        public static void Log_Info_Insert(string info, string type)
        {
            string query = @"insert into LogInfo(info, type) values(@Info, @Type)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Info", info));
            pars.Add(new SQLiteParameter("Type", type));
            ExecuteNonQuery(query, pars);
        }

        public static void Code_Insert(string code, string output, int lang, int editor, string guid, int? userId, string error, string warning, bool show_warnings, int status, string stats)
        {
            string query = @"insert into Code(code, output, lang, editor, guid, user_id, error, warning, show_warnings, status, stats) values(@Code, @Output, @Lang, @Editor, @Guid, @UserId, @Error, @Warning, @ShowWarnings, @Status, @Stats)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Code", code));
            pars.Add(new SQLiteParameter("Output", output));
            pars.Add(new SQLiteParameter("Lang", lang));
            pars.Add(new SQLiteParameter("Editor", editor));
            pars.Add(new SQLiteParameter("Guid", guid));
            pars.Add(new SQLiteParameter("Error", error));
            pars.Add(new SQLiteParameter("Warning", warning));
            pars.Add(new SQLiteParameter("ShowWarnings", show_warnings));
            pars.Add(new SQLiteParameter("Status", status));
            pars.Add(new SQLiteParameter("Stats", stats));
            if(userId == null)
                pars.Add(new SQLiteParameter("UserId", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("UserId", userId));
            ExecuteNonQuery(query, pars);
        }

        public static List<Dictionary<string, object>> Code_Get(string guid)
        {
            string query = @"select * from Code where guid = @Guid";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> Regex_Get(string guid)
        {
            string query = @"select * from Regex where guid = @Guid";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);            
        }

        public static void Regex_Insert(string regex, string text, string output, string options, string guid, int? userId)
        {
            string query = @"insert into Regex(regex, text, output, options, guid, user_id) values(@Regex, @Text, @Output, @Options, @Guid, @UserId)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Regex", regex));
            pars.Add(new SQLiteParameter("Text", text));
            pars.Add(new SQLiteParameter("Output", output));
            pars.Add(new SQLiteParameter("Options", options));
            pars.Add(new SQLiteParameter("Guid", guid));
            if (userId == null)
                pars.Add(new SQLiteParameter("UserId", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("UserId", userId));
            ExecuteNonQuery(query, pars);
        }

        public static List<Dictionary<string, object>> Regex_Replace_Get(string guid)
        {
            string query = @"select * from RegexReplace where guid = @Guid";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Guid", guid));
            return ExecuteQuery(query, pars);
        }

        public static void Regex_Replace_Insert(string regex, string replacement, string text, string output, string options, string guid, int? userId)
        {
            string query = @"insert into RegexReplace(regex, replacement, text, output, options, guid, user_id) values(@Regex, @Replacement, @Text, @Output, @Options, @Guid, @UserId)";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("Regex", regex));
            pars.Add(new SQLiteParameter("Replacement", replacement));
            pars.Add(new SQLiteParameter("Text", text));
            pars.Add(new SQLiteParameter("Output", output));
            pars.Add(new SQLiteParameter("Options", options));
            pars.Add(new SQLiteParameter("Guid", guid));
            if (userId == null)
                pars.Add(new SQLiteParameter("UserId", DBNull.Value));
            else
                pars.Add(new SQLiteParameter("UserId", userId));
            ExecuteNonQuery(query, pars);
        }

        public static List<Dictionary<string, object>> GetUsersCode(int userId)
        {
            string query = @"select code, lang, editor, guid, date, status from Code where user_id = @UserId";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("UserId", userId));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> GetUsersRegex(int userId)
        {
            string query = @"select regex, text, options, date, guid from Regex where user_id = @UserId";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("UserId", userId));
            return ExecuteQuery(query, pars);
        }

        public static List<Dictionary<string, object>> GetUsersRegexReplace(int userId)
        {
            string query = @"select regex, replacement, text, options, date, guid from RegexReplace where user_id = @UserId";
            var pars = new List<SQLiteParameter>();
            pars.Add(new SQLiteParameter("UserId", userId));
            return ExecuteQuery(query, pars);
        }

        static List<Dictionary<string, object>> ExecuteQuery(string query, List<SQLiteParameter> pars)
        {
            List<Dictionary<string, object>> res = new List<Dictionary<string, object>>();
            using (var Conn = GetConnection())
            {
                Conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, Conn))
                {
                    foreach (var par in pars)
                        command.Parameters.Add(par);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                res.Add(new Dictionary<string, object>());
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    res[res.Count - 1][reader.GetName(i)] = reader[i];
                                }
                            }
                        } 
                    }                    
                }
            }
            return res;
        }
        static void ExecuteNonQuery(string query, List<SQLiteParameter> pars)
        {
            using (var Conn = GetConnection())
            {
                Conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, Conn))
                {
                    foreach (var par in pars)
                        command.Parameters.Add(par);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}