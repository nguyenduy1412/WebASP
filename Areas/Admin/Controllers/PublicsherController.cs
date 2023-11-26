using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHS.Areas.Admin.Controllers
{
   /* [Authorize]*/
    public class PublicsherController : Controller
    {
        QLHS.Models.HieuSachEntities db = new QLHS.Models.HieuSachEntities();
        // GET: Admin/Category
        public ActionResult Index()
        {
            List<Models.publicsher> data = db.publicsher.ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(QLHS.Models.publicsher obj)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    db.publicsher.Add(obj);
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
            QLHS.Models.publicsher existingcategory = db.publicsher.Find(id);

            if (existingcategory == null)
            {
                return HttpNotFound();
            }

            return View(existingcategory);
        }

        [HttpPost]
        public ActionResult Edit(QLHS.Models.publicsher obj)
        {
            if (ModelState.IsValid)
            {
                try
                {


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
            QLHS.Models.publicsher a = db.publicsher.Find(id);

            if (a == null)
            {
                return HttpNotFound();
            }

            db.publicsher.Remove(a);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}