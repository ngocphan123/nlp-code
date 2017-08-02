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
    [Authorize(Roles = "Administrator, Manager, Writer, Writers")]
    public class KeyWordsController : Controller
    {
        lCMSData db = new lCMSData();
        //
        // GET: /AdminIT/KeyWords/
        public ActionResult Index()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);
           
            var model = db.KeyWords.Where(e=>e.Type==0).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            IEnumerable<GroupWord> data = (IEnumerable<GroupWord>)db.GroupWords.ToList();
            ViewBag.GroupWord = new SelectList(data, "Id", "Word");

            return View();
        }

        //
        // POST: /AdminIT/GroupComent/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, KeyWord model)
        {
            try
            {
                model.Id = Public.GetID2();
                model.Word = collection["Word"];
                model.GroupWordId = collection["GroupWordId"];
                if (db.KeyWords.Where(e => e.Word.Trim().ToLower().Equals(model.Word) && e.GroupWordId.Trim() == model.GroupWordId).ToList().Count <= 0)
                {
                    model.Type = 0;
                    model.C1 = 0;
                    model.C2 = 0;
                    model.C3 = 0;
                    model.C4 = 0;
                    model.C5 = 0;
                    model.P1 = 0;
                    model.P2 = 0;
                    model.P3 = 0;
                    model.P4 = 0;
                    model.P5 = 0;

                    db.KeyWords.Add(model);
                    db.SaveChanges();
                    TempData["success"] = "Thêm thành công.";
                }
                return RedirectToAction("List");
            }
            catch
            {
                TempData["error"] = "Thêm lỗi.";
                return RedirectToAction("List");
            }
        }

        public ActionResult Edit(string id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            IEnumerable<GroupWord> data = (IEnumerable<GroupWord>)db.GroupWords.ToList();
            ViewBag.GroupWord = new SelectList(data, "Id", "Word");

            var data2 = db.KeyWords.Find(id);

            return View(data2);
        }

        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                var model = db.KeyWords.Find(id);
                model.Word = collection["Word"];
                model.GroupWordId = collection["GroupWordId"];

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                TempData["success"] = "Sửa thành công.";

                return RedirectToAction("List");
            }
            catch
            {
                TempData["error"] = "Sửa lỗi.";
                return RedirectToAction("List");
            }
        }

        public ActionResult Delete(string id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            var data = db.KeyWords.Find(id);
            ViewBag.Meg = data.Word;
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                var data = db.KeyWords.Find(id);
                db.KeyWords.Remove(data);

                db.SaveChanges();

                TempData["success"] = "Xóa thành công.";

                return RedirectToAction("List");
            }
            catch
            {
                TempData["error"] = "Xóa lỗi.";
                return RedirectToAction("List");
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
