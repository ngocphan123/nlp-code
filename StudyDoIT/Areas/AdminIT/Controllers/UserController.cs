using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using StudyDoIT.Models;
using StudyDoIT.Models.NLP;
using StudyDoIT.Models.Common;
using System.IO;

namespace StudyDoIT.Areas.AdminIT.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        lCMSData db = new lCMSData();

        //
        // GET: /AdminIT/User/
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

            var model = db.Users;
            return View(model);
        }

        //
        // GET: /AdminIT/User/Create
        public ActionResult Create()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            IEnumerable<Role> role = (IEnumerable<Role>)db.Roles;
            ViewBag.Roles = new SelectList(role, "Id", "Name");

            return View();
        }

        //
        // POST: /AdminIT/User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, User model)
        {
            //try
            //{
                string username = collection["UserName"];
                string email = collection["Email"];

                var user = db.Users.Where(e => e.UserName == username || e.Email == email).ToList();
                if (user.Count > 0)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    model.FullName = collection["FullName"];
                    model.UserName = username;
                    model.PasswordHash = Public.SHA256(collection["Password1"]);
                    model.Email = email;
                    model.EmailConfirmaed = true;
                    model.Sex = "";
                    model.Address = "";
                    model.PhoneNumber = "";
                    model.SecurityStamp = "";
                    model.PhoneNumberConfirmed = false;
                    model.LockoutEnabled = false;
                    model.RoleId = collection["RoleId"];

                    model.Birthday = DateTime.Now;
                    model.LockoutEndDateUtc = DateTime.Now;

                    string[] img = collection["Photo"].ToString().Split('/');
                    string image = "";
                    for (int i = 3; i < img.Count(); i++)
                    {
                        image += "/" + img[i];
                    }
                    model.Avata = image;

                    if (collection["Lock"] == "on")
                        model.Lock = true;
                    else
                        model.Lock = false;


                    db.Users.Add(model);
                    db.SaveChanges();

                    return RedirectToAction("List");
                }
            //}
            //catch
            //{
            //    return RedirectToAction("List");
            //}
        }

        //
        // GET: /AdminIT/User/Edit/5
        public ActionResult Edit(int id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            var users = db.Users.Find(id);
            IEnumerable<Role> role = (IEnumerable<Role>)db.Roles;
            ViewBag.Roles = new SelectList(role, "Id", "Name", users.RoleId);

            ViewBag.Sex = users.Sex;

            return View(users);
        }

        //
        // POST: /AdminIT/User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var model = db.Users.Find(id);
            
            model.FullName = collection["FullName"];
            model.Email = collection["Email"];
            model.Sex = collection["Sex"];
            model.Address = collection["Address"];
            model.PhoneNumber = collection["PhoneNumber"];

            string[] img = collection["Photo"].ToString().Split('/');
            if (img.Count() > 1)
            {
                if (img[1] != "Uploads")
                {
                    model.Avata = collection["Images"];
                }
                else
                {
                    string image = "";
                    for (int i = 3; i < img.Count(); i++)
                    {
                        image += "/" + img[i];
                    }
                    model.Avata = image;
                }
            }

            if (collection["Lock"] == "on")
                model.Lock = true;
            else
                model.Lock = false;

            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("List");
        }

        //
        // GET: /AdminIT/User/Delete/5
        public ActionResult Delete(int id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            var user = db.Users.Find(id);
            ViewBag.Meg = user.UserName;
            return View();
        }

        //
        // POST: /AdminIT/User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var category = db.Categories.Where(e => e.UserId == id).Select(e => e.Id);

            foreach (int c in category)
            {
                var cate = db.Categories.Find(c);

                //var news = db.CategoryNews.Where(e => e.CategoryId == cate.Id).Select(e => e.Id);
                //foreach (string s in news)
                //{
                //    var tg = db.News.Find(s);

                //    var cm = db.Comments.Where(e => e.NewsId == tg.Id).Select(e => e.Id);
                //    foreach (string s1 in cm)
                //    {
                //        var tg1 = db.Comments.Find(s1);
                //        db.Comments.Remove(tg1);
                //        db.SaveChanges();
                //    }

                //    var t = db.Tags.Where(e => e.NewsId == tg.Id).Select(e => e.Id);
                //    foreach (string s2 in t)
                //    {
                //        var tg2 = db.Tags.Find(s2);
                //        db.Tags.Remove(tg2);
                //        db.SaveChanges();
                //    }

                //    var c1 = db.CategoryNews.Where(e => e.NewsId == tg.Id).Select(e => e.Id);
                //    foreach (string s3 in c1)
                //    {
                //        var tg3 = db.CategoryNews.Find(s3);
                //        db.CategoryNews.Remove(tg3);
                //        db.SaveChanges();
                //    }

                //    var sl = db.Slides.Where(e => e.NewsId == tg.Id).Select(e => e.Id);
                //    foreach (var s4 in sl)
                //    {
                //        var tg4 = db.Slides.Find(s4);
                //        db.Slides.Remove(tg4);
                //        db.SaveChanges();
                //    }

                //    var lo = db.Locations.Where(e => e.NewsId == tg.Id).Select(e => e.Id);
                //    foreach (var s5 in lo)
                //    {
                //        var tg5 = db.Locations.Find(s5);
                //        db.Locations.Remove(tg5);
                //        db.SaveChanges();
                //    }

                //    db.News.Remove(tg);
                //    db.SaveChanges();
                //}

                db.Categories.Remove(cate);
                db.SaveChanges();
            }


            //var news1 = db.News.Where(e => e.UserId == id).Select(e => e.Id);
            //foreach (string s in news1)
            //{
            //    var tg = db.News.Find(s);

            //    var cm = db.Comments.Where(e => e.NewsId == tg.Id).Select(e => e.Id);
            //    foreach (string s1 in cm)
            //    {
            //        var tg1 = db.Comments.Find(s1);
            //        db.Comments.Remove(tg1);
            //        db.SaveChanges();
            //    }

            //    var t = db.Tags.Where(e => e.NewsId == tg.Id).Select(e => e.Id);
            //    foreach (string s2 in t)
            //    {
            //        var tg2 = db.Tags.Find(s2);
            //        db.Tags.Remove(tg2);
            //        db.SaveChanges();
            //    }

            //    //var c1 = db.CategoryNews.Where(e => e.NewsId == tg.Id).Select(e => e.Id);
            //    //foreach (string s3 in c1)
            //    //{
            //    //    var tg3 = db.CategoryNews.Find(s3);
            //    //    db.CategoryNews.Remove(tg3);
            //    //    db.SaveChanges();
            //    //}

            //    //var sl = db.Slides.Where(e => e.NewsId == tg.Id).Select(e => e.Id);
            //    //foreach (string s4 in sl)
            //    //{
            //    //    var tg4 = db.Slides.Find(s4);
            //    //    db.Slides.Remove(tg4);
            //    //    db.SaveChanges();
            //    //}

            //    db.News.Remove(tg);
            //    db.SaveChanges();
            //}

            var user = db.Users.Find(id);
            db.Users.Remove(user);
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
