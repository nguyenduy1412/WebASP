using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHS.Areas.Admin.Controllers
{
  /*  [Authorize]*/
    public class AuthorController : Controller
    {
        QLHS.Models.HieuSachEntities db = new QLHS.Models.HieuSachEntities();
        // GET: Admin/Category
        public ActionResult Index()
        {
            var us = QLHS.App_Start.SessionConfig.GetUser();
            if (us == null)
            {
                return RedirectToAction("Index", "Login");
            }
            List<Models.author> data = db.author.ToList();
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
        public ActionResult Create(QLHS.Models.author obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var fImage = Request.Files["fImage"];
                    if (fImage != null && fImage.ContentLength > 0)
                    {
                        string fName = fImage.FileName;
                        string foder = Server.MapPath("~/Assets/Upload/" + fName);
                        fImage.SaveAs(foder);
                        obj.img =  fName;
                    }
                    db.author.Add(obj);
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
            QLHS.Models.author existingcategory = db.author.Find(id);

            if (existingcategory == null)
            {
                return HttpNotFound();
            }

            return View(existingcategory);
        }

        [HttpPost]
        public ActionResult Edit(QLHS.Models.author obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var fImage = Request.Files["fImage"];
                    if (fImage != null && fImage.ContentLength > 0)
                    {

                        string fName = fImage.FileName;
                        string folder = Server.MapPath("~/Assets/Upload/" + fName);
                        fImage.SaveAs(folder);
                        obj.img =  fName;
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
            QLHS.Models.author categoryToDelete = db.author.Find(id);

            if (categoryToDelete == null)
            {
                return HttpNotFound();
            }

            db.author.Remove(categoryToDelete);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}