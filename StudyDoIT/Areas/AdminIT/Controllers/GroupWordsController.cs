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
    public class GroupWordsController : Controller
    {
        lCMSData db = new lCMSData();
        //
        // GET: /AdminIT/GroupWords/
        public ActionResult Index()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;
            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);
            return RedirectToAction("List");
        }

        public ActionResult List(string id = "")
        {
            IEnumerable<Product> data = (IEnumerable<Product>)db.Products.ToList();
            if (id == "")
            {
                ViewBag.Product = new SelectList(data, "Id", "Name");
            }
            else
            {
                ViewBag.Product = new SelectList(data, "Id", "Name", id);
            }

            ViewBag.Id = id;
            var model = db.GroupWords.ToList();
            return View(model);
        }

        public ActionResult LoadListGroupWord(string idp = "")
        {
            var data = db.GroupWords.Where(e => e.ProductId == idp).ToList();
            return PartialView("_ListGroupWord", data);
        }

        //
        // GET: /AdminIT/GroupWords/Create
        public ActionResult Create(string id = "")
        {
            Session["current_url"] = Request.Url.AbsoluteUri;
            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            IEnumerable<Product> data = (IEnumerable<Product>)db.Products.Where(e => e.Publish == 1).ToList();
            if (id == "")
            {
                ViewBag.Product = new SelectList(data, "Id", "Name");
            }
            else
            {
                ViewBag.Product = new SelectList(data, "Id", "Name", id);
            }
            return View();
        }

        //
        // POST: /AdminIT/GroupWords/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, GroupWord model)
        {
            try
            {
                model.Id = Public.GetID2();
                model.Word = collection["Word"];
                model.C1 = 0;
                model.C2 = 0;
                model.C3 = 0;
                model.C4 = 0;
                model.C5 = 0;
                model.ProductId = collection["ProductId"];


                db.GroupWords.Add(model);
                db.SaveChanges();
                TempData["success"] = "Thêm thành công.";

                return RedirectToAction("List", new { id=model.ProductId});
            }
            catch
            {
                TempData["error"] = "Thêm lỗi.";
                return RedirectToAction("List");
            }
        }

        //
        // GET: /AdminIT/GroupWords/Edit/5
        public ActionResult Edit(string id="")
        {
            Session["current_url"] = Request.Url.AbsoluteUri;
            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            IEnumerable<Product> data2 = (IEnumerable<Product>)db.Products.ToList();
            if (id == "")
            {
                ViewBag.Product = new SelectList(data2, "Id", "Name");
            }
            else
            {
                ViewBag.Product = new SelectList(data2, "Id", "Name", id);
            }

            var data = db.GroupWords.Find(id);

            return View(data);
        }

        //
        // POST: /AdminIT/GroupWords/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                var model = db.GroupWords.Find(id);
                model.Word = collection["Word"];
                model.ProductId = collection["ProductId"];

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                TempData["success"] = "Sửa thành công.";

                return RedirectToAction("List", new { id = model.ProductId });
            }
            catch
            {
                TempData["error"] = "Sửa lỗi.";
                return RedirectToAction("List");
            }
        }

        //
        // GET: /AdminIT/GroupWords/Delete/5
        public ActionResult Delete(string id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            var data = db.GroupWords.Find(id);
            ViewBag.Meg = data.Word;
            return View();
        }

        //
        // POST: /AdminIT/GroupWords/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                var c1 = db.LogLabels.Where(e => e.GroupKeywords == id).Select(e => e.Id);
                foreach (string s in c1)
                {
                    var cn = db.LogLabels.Find(s);
                    db.LogLabels.Remove(cn);
                }
                db.SaveChanges();

                var c2 = db.LogAddKeyWords.Where(e => e.GroupWordId == id).Select(e => e.Id);
                foreach (string s in c2)
                {
                    var cn = db.LogAddKeyWords.Find(s);
                    db.LogAddKeyWords.Remove(cn);
                }
                db.SaveChanges();

                var c3 = db.SeKeyWords.Where(e => e.KeyWordId == id).Select(e => e.Id);
                foreach (string s in c3)
                {
                    var cn = db.SeKeyWords.Find(s);
                    db.SeKeyWords.Remove(cn);
                }
                db.SaveChanges();

                var c4 = db.KeywordsCounts.Where(e => e.GroupKeyWordId == id).ToList();
                foreach (var s in c4)
                {
                    //var cn = db.KeywordsCounts.Find(s.);
                    db.KeywordsCounts.Remove(s);
                }
                db.SaveChanges();

                var c8 = db.KeyWords.Where(e => e.GroupWordId == id).Select(e => e.Id);
                foreach (string s in c8)
                {
                    var cn = db.KeyWords.Find(s);
                    db.KeyWords.Remove(cn);
                }
                db.SaveChanges();

                //Xóa sản phẩm
                var data = db.GroupWords.Find(id);
                string idp = data.ProductId;
                db.GroupWords.Remove(data);

                db.SaveChanges();

                TempData["success"] = "Xóa thành công.";
                return RedirectToAction("List", new { id = idp });
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
