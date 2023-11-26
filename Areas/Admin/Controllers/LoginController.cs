using QLHS.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QLHS.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        Models.HieuSachEntities db = new Models.HieuSachEntities();
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Models.users obj)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword;
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hashedPasswordBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(obj.pass_word));
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashedPasswordBytes)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    hashedPassword = sb.ToString();
                }
                Models.users check = db.users.FirstOrDefault(m => m.user_name == obj.user_name && m.pass_word == hashedPassword);
                if (check == null)
                {
                    //Đăng nhập thất bại
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng!");
                }
                else
                {
                    SessionConfig.SetUser(obj);
                    var us = SessionConfig.GetUser();
                    FormsAuthentication.SetAuthCookie(check.user_name, false);
                    if (Request.QueryString["ReturnUrl"]==null || Request.QueryString["ReturnUrl"] == "")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(Request.QueryString["ReturnUrl"]);
                    }
                }
            }
            return View(obj);
        }
        public ActionResult Logout()
        {
            SessionConfig.SetUser(null);
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Models.users model, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                // Check if the username is already taken
                if (db.users.Any(u => u.user_name == model.user_name))
                {
                    ModelState.AddModelError("user_name", "Username is already taken. Please choose a different one.");
                    return View(model);
                }
                if (model.pass_word != confirmPassword)
                {
                    ModelState.AddModelError("confirmPassword", "The password and confirmation password do not match.");
                    return View(model);
                }
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hashedPasswordBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(model.pass_word));
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashedPasswordBytes)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    model.pass_word = sb.ToString();
                }
                db.users.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

    }
}