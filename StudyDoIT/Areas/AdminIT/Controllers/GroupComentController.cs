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
    public class GroupComentController : Controller
    {
        lCMSData db = new lCMSData();

        //
        // GET: /AdminIT/GroupComent/
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
            var model = db.GroupComents.Where(e=>e.ProductId==id).ToList();
            return View(model);
        }

        public ActionResult LoadListGComment(string idp="")
        {
            var data = db.GroupComents.Where(e => e.ProductId == idp).ToList();
            return PartialView("_ListGComment", data);
        }

        //
        // GET: /AdminIT/GroupComent/Create
        public ActionResult Create(string id = "")
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            IEnumerable<Product> data = (IEnumerable<Product>)db.Products.Where(e=>e.Publish==1).ToList();
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
        // POST: /AdminIT/GroupComent/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, GroupComent model)
        {
            try
            {
                model.Id = Public.GetID2();
                model.Name = collection["Name"];

                if (collection["NameProduct"] != "")
                {
                    Product model2 = new Product();
                    model2.Id = Public.GetID2();
                    model2.Name = collection["NameProduct"];
                    model2.Publish = 1;
                    db.Products.Add(model2);
                    db.SaveChanges();

                    model.ProductId = model2.Id;
                }
                else
                {
                    model.ProductId = collection["ProductId"];
                }

                db.GroupComents.Add(model);
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
        // GET: /AdminIT/GroupComent/Edit/5
        public ActionResult Edit(string id)
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

            var data = db.GroupComents.Find(id);

            return View(data);
        }

        //
        // POST: /AdminIT/GroupComent/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                var model = db.GroupComents.Find(id);
                model.Name = collection["Name"];
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
        // GET: /AdminIT/GroupComent/Delete/5
        public ActionResult Delete(string id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            var data = db.GroupComents.Find(id);
            ViewBag.Meg = data.Name;
            return View();
        }

        //
        // POST: /AdminIT/GroupComent/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                var c1 = db.LogLabels.Where(e => e.GroupCommentId == id).Select(e => e.Id);
                foreach (string s in c1)
                {
                    var cn = db.LogLabels.Find(s);
                    db.LogLabels.Remove(cn);
                }
                db.SaveChanges();

                var c2 = db.LogAddKeyWords.Where(e => e.GroupCommentId == id).Select(e => e.Id);
                foreach (string s in c2)
                {
                    var cn = db.LogAddKeyWords.Find(s);
                    db.LogAddKeyWords.Remove(cn);
                }
                db.SaveChanges();

                var c3 = db.SeKeyWords.Where(e => e.Sentens.Comment.GroupCommentId == id).Select(e => e.Id);
                foreach (string s in c3)
                {
                    var cn = db.SeKeyWords.Find(s);
                    db.SeKeyWords.Remove(cn);
                }
                db.SaveChanges();

                var c4 = db.KeywordsCounts.Where(e => e.GroupCommentId == id).ToList();
                foreach (var s in c4)
                {
                    //var cn = db.KeywordsCounts.Find(s.);
                    db.KeywordsCounts.Remove(s);
                }
                db.SaveChanges();

                var c5 = db.Vocabularies.Where(e => e.GroupCommentId == id).Select(e => e.Id);
                foreach (string s in c5)
                {
                    var cn = db.Vocabularies.Find(s);
                    db.Vocabularies.Remove(cn);
                }
                db.SaveChanges();

                var c6 = db.Sentenses.Where(e => e.Comment.GroupCommentId == id).Select(e => e.Id);
                foreach (string s in c6)
                {
                    var cn = db.Sentenses.Find(s);
                    db.Sentenses.Remove(cn);
                }
                db.SaveChanges();

                var c7 = db.Comments.Where(e => e.GroupCommentId == id).Select(e => e.Id);
                foreach (string s in c7)
                {
                    var cn = db.Comments.Find(s);
                    db.Comments.Remove(cn);
                }
                db.SaveChanges();

                var c8 = db.KeyWords.Where(e => e.GroupCommentId == id).Select(e => e.Id);
                foreach (string s in c8)
                {
                    var cn = db.KeyWords.Find(s);
                    db.KeyWords.Remove(cn);
                }
                db.SaveChanges();

                //Xóa sản phẩm
                var data = db.GroupComents.Find(id);
                db.GroupComents.Remove(data);

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
