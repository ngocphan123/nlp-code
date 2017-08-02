using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using StudyDoIT.Models.NLP;
using StudyDoIT.Models.Common;
using System.IO;

namespace StudyDoIT.Areas.AdminIT.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TypeMenuController : Controller
    {
        //
        // GET: /AdminIT/TypeMenu/

        lCMSData db = new lCMSData();

        public ActionResult Index()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            var model = db.TypeMenus;
            return View(model);
        }

        //
        // GET: /AdminIT/TypeMenu/Details/5
        public ActionResult Details(int id)
        {
            

            return View();
        }

        //
        // GET: /AdminIT/TypeMenu/Create
        public ActionResult Create()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            return View();
        }

        //
        // POST: /AdminIT/TypeMenu/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, TypeMenu model)
        {
            //if (model != null && ModelState.IsValid)
            //{
            //try
            //{
            model.Id = Public.GetID();
            model.Name = collection["Name"];
            model.Publich = int.Parse(collection["Publich"]);

            //if (collection["Publich"] == "true")
            //    model.Publich = 1;
            //else
            //    model.Publich = 0;

            db.TypeMenus.Add(model);
            db.SaveChanges();
            //}

            return RedirectToAction("List");
            //return View();
            //}
            //catch
            //{
            //    ModelState.AddModelError("", "Error");
            //}
        }

        //
        // GET: /AdminIT/TypeMenu/Edit/5
        public ActionResult Edit(string id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            var data = db.TypeMenus.Find(id);

            return View(data);
        }

        //
        // POST: /AdminIT/TypeMenu/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection, TypeMenu model)
        {
            
            model.Id = collection["Id"];
            model.Name = collection["Name"];
            model.Publich = int.Parse(collection["Publich"]);

            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("List");
        }

        //
        // GET: /AdminIT/TypeMenu/Delete/5
        public ActionResult Delete(string id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            var data = db.TypeMenus.Find(id);
            ViewBag.Meg = data.Name;

            return View();
        }

        //
        // POST: /AdminIT/TypeMenu/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {

            var data = db.TypeMenus.Find(id);
            db.TypeMenus.Remove(data);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
