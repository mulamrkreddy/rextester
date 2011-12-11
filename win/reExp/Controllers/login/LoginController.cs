using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reExp.Models;
using reExp.Utils;

namespace reExp.Controllers.login
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        [ValidateInput(false)]
        public ActionResult Index(LoginData data)
        {
            Compression.SetCompression();
            if ((string.IsNullOrEmpty(data.Name) || string.IsNullOrEmpty(data.Password)) &&
               (string.IsNullOrEmpty(data.RegName) || string.IsNullOrEmpty(data.RegPassword)))
            return View(data);

            

            if (!string.IsNullOrEmpty(data.Name) && !string.IsNullOrEmpty(data.Password))
            {
                var res = Model.LoginUser(data.Name, data.Password);
                if (res.NoSuchUser)
                {
                    data.IsError = true;
                    data.Error = "No such user name.";
                    return View(data);
                }
                if (res.BadPassword)
                {
                    data.IsError = true;
                    data.Error = "Incorrect password.";
                    return View(data);
                }
                if (!string.IsNullOrEmpty(res.Error))
                {
                    data.IsError = true;
                    data.Error = res.Error;
                    return View(data);
                }
                return this.RedirectToAction("UsersStuff");
            }

            if (!string.IsNullOrEmpty(data.RegName) && !string.IsNullOrEmpty(data.RegPassword))
            {
                if (data.RegName.Length > 100 || data.RegPassword.Length > 100)
                {
                    data.IsError = true;
                    data.Error = "User name and password should be shorter than 100 characters.";
                    return View(data);
                }
                var res = Model.RegisterUser(data.RegName, data.RegPassword);
                if (res.NameTaken)
                {
                    data.IsError = true;
                    data.Error = "This name already taken.";
                    return View(data);
                }
                if (!string.IsNullOrEmpty(res.Error))
                {
                    data.IsError = true;
                    data.Error = res.Error;
                    return View(data);
                }
                return this.RedirectToAction("UsersStuff");
            }

            return View(data);
        }

        public ActionResult UsersStuff()
        {
            Compression.SetCompression();
            UserData data = new UserData();
            if(!SessionManager.IsUserInSession())
            {
                data.Error = "User not loged in";
                data.IsError = true;
                return View(data);
            }
            data.UserName = SessionManager.UserName;
            int userId = (int)SessionManager.UserId;
            data.Snippets = Model.GetUsersCode(userId);
            data.Regexes = Model.GetUsersRegex(userId);
            data.Replaces = Model.GetUsersRegexReplace(userId);
            return View(data);
        }

        public ActionResult Logout()
        {
            SessionManager.LogOut();
            return this.RedirectToAction("Index");
        }

    }
}
