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
    public class ProductsController : Controller
    {
        lCMSData db = new lCMSData();

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

            var model = db.Products.ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, Product model)
        {
            try
            {
                model.Id = Public.GetID2();
                model.Name = collection["Name"];
                if (collection["Publish"] == "on")
                {
                    model.Publish = 1;
                }
                else
                {
                    model.Publish = 0;
                }

                db.Products.Add(model);
                db.SaveChanges();
                TempData["success"] = "Thêm thành công.";

                return RedirectToAction("List");
            }
            catch
            {
                TempData["error"] = "Thêm lỗi.";
                return RedirectToAction("List");
            }
        }

        public JsonResult AddProduct(string name, string type)
        {
            try
            {
                Product model = new Product();
                model.Id = Public.GetID2();
                model.Name = name;
                if (type == "on")
                {
                    model.Publish = 1;
                }
                else
                {
                    model.Publish = 0;
                }

                db.Products.Add(model);
                db.SaveChanges();
                return Json("Thêm thành công");
            }
            catch
            {
                return Json("Thêm lỗi");
            }
        }

        public ActionResult Edit(string id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            var data = db.Products.Find(id);

            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                var model = db.Products.Find(id);
                model.Name = collection["Name"];
                if (collection["Publish"] == "on")
                {
                    model.Publish = 1;
                }
                else
                {
                    model.Publish = 0;
                }

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

            var data = db.Products.Find(id);
            ViewBag.Meg = data.Name;
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                var c8 = db.GroupComents.Where(e => e.ProductId == id).ToList();
                foreach (var s8 in c8)
                {
                    var c1 = db.LogLabels.Where(e => e.GroupCommentId == s8.Id).Select(e => e.Id);
                    foreach (string s in c1)
                    {
                        var cn = db.LogLabels.Find(s);
                        db.LogLabels.Remove(cn);
                    }
                    db.SaveChanges();

                    var c2 = db.LogAddKeyWords.Where(e => e.GroupCommentId == s8.Id).Select(e => e.Id);
                    foreach (string s in c2)
                    {
                        var cn = db.LogAddKeyWords.Find(s);
                        db.LogAddKeyWords.Remove(cn);
                    }
                    db.SaveChanges();

                    var c3 = db.SeKeyWords.Where(e => e.Sentens.Comment.GroupCommentId == s8.Id).Select(e => e.Id);
                    foreach (string s in c3)
                    {
                        var cn = db.SeKeyWords.Find(s);
                        db.SeKeyWords.Remove(cn);
                    }
                    db.SaveChanges();

                    var c4 = db.KeywordsCounts.Where(e => e.GroupCommentId == s8.Id).ToList();
                    foreach (var s in c4)
                    {
                        db.KeywordsCounts.Remove(s);
                    }
                    db.SaveChanges();

                    var c5 = db.Vocabularies.Where(e => e.GroupCommentId == s8.Id).Select(e => e.Id);
                    foreach (string s in c5)
                    {
                        var cn = db.Vocabularies.Find(s);
                        db.Vocabularies.Remove(cn);
                    }
                    db.SaveChanges();

                    var c6 = db.Sentenses.Where(e => e.Comment.GroupCommentId == s8.Id).Select(e => e.Id);
                    foreach (string s in c6)
                    {
                        var cn = db.Sentenses.Find(s);
                        db.Sentenses.Remove(cn);
                    }
                    db.SaveChanges();

                    var c7 = db.Comments.Where(e => e.GroupCommentId == s8.Id).Select(e => e.Id);
                    foreach (string s in c7)
                    {
                        var cn = db.Comments.Find(s);
                        db.Comments.Remove(cn);
                    }
                    db.SaveChanges();

                    db.GroupComents.Remove(s8);
                }
                db.SaveChanges();

                //Xóa keyword
                var c10 = db.GroupWords.Where(e => e.ProductId == id).ToList();
                foreach (var s10 in c10)
                {
                    var c9 = db.KeyWords.Where(e => e.GroupWordId == s10.Id).Select(e => e.Id);
                    foreach (string s in c9)
                    {
                        var cn = db.KeyWords.Find(s);
                        db.KeyWords.Remove(cn);
                    }
                    db.SaveChanges();

                    db.GroupWords.Remove(s10);
                }
                db.SaveChanges();

                //Xóa sản phẩm
                var data = db.Products.Find(id);
                db.Products.Remove(data);

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
