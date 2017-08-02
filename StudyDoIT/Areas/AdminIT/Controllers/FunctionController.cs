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
    public class FunctionController : Controller
    {
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

            var model = db.Categories.Where(e => e.Type == 4);
            return View(model);
        }

        public ActionResult Create()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            IEnumerable<Category> categorys = (IEnumerable<Category>)db.Categories.Where(e => e.Type == 4 && e.Publish == 1);
            ViewBag.Category = new SelectList(categorys, "Id", "Name");
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(FormCollection collection, Category model)
        {
            try
            {
                model.Name = collection["Name"];
                model.Description = collection["Description"];
                model.Url = collection["Url"];

                model.CategoryParentId = collection["CategoryParentId"] == "" ? 0 : int.Parse(collection["CategoryParentId"]);
                model.DatePublish = DateTime.Now;
                model.DateUpdate = DateTime.Now;
                model.UserId = int.Parse(Session["Ad_Id"].ToString());
                model.Location = int.Parse(collection["Location"]);

                string[] img = collection["Images"].ToString().Split('/');
                string image = "";
                for (int i = 3; i < img.Count(); i++)
                {
                    image += "/" + img[i];
                }
                model.Images = image;

                if (collection["Publish"] == "on")
                    model.Publish = 1;
                else
                    model.Publish = 0;

                model.Type = 4;
                model.MetaTitle = "";
                model.MetaKeyword = "";
                model.MetaDescrption = "";

                db.Categories.Add(model);
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

        public ActionResult Edit(int id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            var category = db.Categories.Find(id);

            IEnumerable<Category> categorys = (IEnumerable<Category>)db.Categories.Where(e => e.Id != id && e.Type == 4 && e.Publish == 1);
            if (category.CategoryParentId == 0)
            {
                ViewBag.Category = new SelectList(categorys, "Id", "Name", "");
            }
            else
            {
                ViewBag.Category = new SelectList(categorys, "Id", "Name", category.CategoryParentId);
            }
            ViewBag.Location = category.Location;
            return View(category);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(FormCollection collection, int id)
        {
            try
            {
                var model = db.Categories.Find(id);
                model.Name = collection["Name"];
                model.Description = collection["Description"];
                model.Url = collection["Url"];
                //string title = db.Categories.AsNoTracking().First(e => e.Id == model.Id).Name;
                //if (title.Trim().ToUpper().Equals(collection["Name"].Trim().ToUpper()))
                //{
                //    model.Url = collection["Url"];
                //}
                //else
                //{
                //    model.Url = Public.NonUnicode(collection["Name"]);
                //}

                //string category = collection["CategoryParentId"];
                model.CategoryParentId = collection["CategoryParentId"] == "" ? 0 : int.Parse(collection["CategoryParentId"]);
                model.DateUpdate = DateTime.Now;
                model.UserId = int.Parse(Session["Ad_Id"].ToString());
                model.Location = int.Parse(collection["Location"]);

                string[] img = collection["Images"].ToString().Split('/');
                if (img.Count() > 1)
                {
                    if (img[1] != "Uploads")
                    {
                        model.Images = collection["Images"];
                    }
                    else
                    {
                        string image = "";
                        for (int i = 3; i < img.Count(); i++)
                        {
                            image += "/" + img[i];
                        }
                        model.Images = image;
                    }
                }

                if (collection["Publish"] == "on")
                    model.Publish = 1;
                else
                    model.Publish = 0;


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

        public ActionResult Delete(int id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            var category = db.Categories.Find(id);
            ViewBag.Meg = category.Name;
            return View();
        }

        //
        // POST: /AdminIT/Tags/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                //var news = db.News.Where(e => e.CategoryId == id);
                //foreach (var s in news)
                //{
                //    //var tg = db.News.Find(s);
                //    var cm = db.Comments.Where(e => e.NewsId == s.Id);
                //    foreach (var s1 in cm)
                //    {
                //        db.Comments.Remove(s1);
                //    }

                //    var t = db.Tags.Where(e => e.NewsId == s.Id);
                //    foreach (var s2 in t)
                //    {
                //        db.Tags.Remove(s2);
                //    }
                //    db.News.Remove(s);
                //}

                //var c1 = db.CategoryNewsMappings.Where(e => e.CategoryId == id);
                //foreach (var s in c1)
                //{
                //    db.CategoryNewsMappings.Remove(s);
                //}

                var category = db.Categories.Find(id);
                db.Categories.Remove(category);

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
