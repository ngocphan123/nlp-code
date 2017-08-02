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
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace StudyDoIT.Areas.AdminIT.Controllers
{
    [Authorize(Roles = "Administrator, Manager")]
    public class MenuController : Controller
    {
        lCMSData db = new lCMSData();
        //
        // GET: /AdminIT/Menu/
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
        // GET: /AdminIT/Menu/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AdminIT/Menu/Create
        public ActionResult Create()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            return View();
        }

        public ActionResult LoadMenuParent(string idMenu)
        {
            IEnumerable<Menu> menu = (IEnumerable<Menu>)db.Menus.Where(e => e.Publish == 1 && e.TypeMenuId == idMenu);
            ViewBag.Menu = new SelectList(menu, "Id", "CategoryId");
            return Json(null);
        }

        //
        // POST: /AdminIT/Menu/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                string output = collection["output2"];

                output = @"{""data"":" + output + "}";

                var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(output);

                int publish;
                TypeMenu tm = new TypeMenu();
                string Id=Public.GetID();
                tm.Id = Id;
                tm.Name = collection["Name"];
                if (collection["Publich"] == "on")
                {
                    tm.Publich = 1;
                    publish=1;
                }
                else
                {
                    tm.Publich = 0;
                    publish=0;
                }
                db.TypeMenus.Add(tm);
                db.SaveChanges();
                MenuItem(jsonResult["data"], Id, "0", publish);

                return RedirectToAction("List");
            }
            catch
            {
                TempData["error"] = "Thêm lỗi.";
                return RedirectToAction("List");
            }
        }

        private void MenuItem(dynamic jsonResult, string typeMenu, string menuParent, int publish)
        {
            for (int i = 0; i < jsonResult.Count; i++)
            {
                string Id = Public.GetID();
                int id = jsonResult[i]["id"];
                if (id != 0)
                {
                    Menu model = new Menu();
                    model.Id = Id;
                    model.CategoryId = id;
                    model.TypeMenuId = typeMenu;
                    model.Location = (i + 1);
                    model.MenuParent = menuParent;
                    model.DatePublish = DateTime.Now;
                    model.DateUpdate = DateTime.Now;
                    model.UserId = int.Parse(Session["Ad_Id"].ToString());
                    model.Publish = publish;
                    db.Menus.Add(model);
                }
                dynamic children = jsonResult[i]["children"];
                if (children != null)
                {
                    MenuItem(children, typeMenu, Id, publish);
                }
            }
            db.SaveChanges();
            TempData["success"] = "Thêm thành công.";
        }


        //
        // GET: /AdminIT/Menu/Edit/5
        public ActionResult Edit(string id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            var data = db.TypeMenus.Find(id);
            //ViewBag.MenuId = id;
            return View(data);
        }

        //
        // POST: /AdminIT/Menu/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection, string id)
        {
            int publish;
            var tm = db.TypeMenus.Find(id);
            if (collection["Publich"] == "on")
            {
                tm.Publich = 1;
                publish = 1;
            }
            else
            {
                tm.Publich = 0;
                publish = 0;
            }

            db.Entry(tm).State = EntityState.Modified;
            db.SaveChanges();

            string output = collection["output2"];

            output = @"{""data"":" + output + "}";

            var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(output);

            var menu = db.Menus.Where(e=>e.TypeMenuId==id);
            foreach (var m in menu)
            {
                db.Menus.Remove(m);
            }
            db.SaveChanges();

            MenuItem(jsonResult["data"], id, "0", publish);

            return RedirectToAction("List");
        }

        //
        // GET: /AdminIT/Menu/Delete/5
        public ActionResult Delete(string id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            var menu = db.TypeMenus.Find(id);
            ViewBag.Meg = menu.Name;
            return View();
        }

        //
        // POST: /AdminIT/Menu/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            var data = db.TypeMenus.Find(id);
            var menu = db.Menus.Where(e => e.TypeMenuId == id);
            foreach (var m in menu)
            {
                db.Menus.Remove(m);
            }
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
