using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHS.Areas.Admin.Controllers
{
   /* [Authorize]*/
    public class BannerController : Controller
    {
        Models.HieuSachEntities db = new Models.HieuSachEntities();
        // GET: Admin/Banner
        public ActionResult Index()
        {
            var us = QLHS.App_Start.SessionConfig.GetUser();
            if (us == null)
            {
                return RedirectToAction("Index", "Login");
            }
            List<Models.banner> data = db.banner.ToList();

            return View(data);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var us = QLHS.App_Start.SessionConfig.GetUser();
            if (us == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(Models.banner obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var fImage = Request.Files["fImage"];
                    if (fImage != null && fImage.ContentLength > 0)
                    {
                        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string fName = timeStamp + "_" + Path.GetFileName(fImage.FileName);
                        string foder = Server.MapPath("~/Assets/Uploads/" + fName);
                        fImage.SaveAs(foder);
                        obj.img = "~/Assets/Uploads/" + fName;
                    }
                    db.banner.Add(obj);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch { }
            }

            return View(obj);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var us = QLHS.App_Start.SessionConfig.GetUser();
            if (us == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Models.banner existingbanner = db.banner.Find(id);

            if (existingbanner == null)
            {
                return HttpNotFound();
            }

            return View(existingbanner);
        }

        [HttpPost]
        public ActionResult Edit(Models.banner obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var fImage = Request.Files["fImage"];
                    if (fImage != null && fImage.ContentLength > 0)
                    {

                        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string fName = timeStamp + "_" + Path.GetFileName(fImage.FileName);
                        string folder = Server.MapPath("~/Assets/Uploads/" + fName);
                        fImage.SaveAs(folder);
                        obj.img = "~/Assets/Uploads/" + fName;
                    }

                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    // Handle exceptions
                }
            }


            return View(obj);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Models.banner bannerToDelete = db.banner.Find(id);

            if (bannerToDelete == null)
            {
                return HttpNotFound();
            }

            db.banner.Remove(bannerToDelete);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}