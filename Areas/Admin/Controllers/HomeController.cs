using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHS.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            var us = QLHS.App_Start.SessionConfig.GetUser();
            if (us==null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (us.users_roles.Where(u => u.role_id==1).FirstOrDefault()!=null)
            {
                return View();
                
            }
            return RedirectToAction("Index", "Login");

        }
    }
}