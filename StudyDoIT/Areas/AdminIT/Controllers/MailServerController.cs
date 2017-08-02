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
    [Authorize(Roles = "Administrator, Manager")]
    public class MailServerController : Controller
    {
        lCMSData db = new lCMSData();
        //
        // GET: /AdminIT/MailServer/
        public ActionResult Index()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            return RedirectToAction("Edit");
        }

        //
        // GET: /AdminIT/MailServer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AdminIT/MailServer/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminIT/MailServer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /AdminIT/MailServer/Edit/5
        public ActionResult Edit(int id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            var home = db.MailServers.Find(id);
            return View(home);
        }

        //
        // POST: /AdminIT/MailServer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var model = db.MailServers.Find(id);
                model.Host = collection["Host"];
                model.SenderMail = collection["SenderMail"];
                if (collection["EnableSSL"] == "on")
                {
                    model.EnableSSL = true;
                }
                else
                {
                    model.EnableSSL = false;
                }
                model.UserName = collection["UserName"];
                model.Password = collection["Password"];
                model.DisplayName = collection["DisplayName"];
                model.Port = collection["Port"];

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            catch
            {
                return RedirectToAction("Edit");
            }
        }

        //
        // GET: /AdminIT/MailServer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /AdminIT/MailServer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
