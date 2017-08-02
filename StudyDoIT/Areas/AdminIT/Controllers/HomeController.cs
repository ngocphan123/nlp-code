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
    //[Authorize(Roles = "Administrator, Manager")]
    public class HomeController : Controller
    {
        lCMSData db = new lCMSData();
        //
        // GET: /AdminIT/Home/
        public ActionResult Index()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;
            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            return View();

        }

        public ActionResult Menu(int id, string seoName)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            try
            {
                var seo = db.Categories.Where(e => e.Id == id && e.Type == 4).First();
                if (seo == null)
                    return HttpNotFound();

                string strSEO = seo.Url;
                if (seoName != strSEO)
                    return RedirectToRoute("DTAdmin", new { id = id, seoName = strSEO });

                return View("DTAdmin", seo);
            }
            catch
            {
                return HttpNotFound();
            }

        }

        public ActionResult LoadHeader()
        {
            //if (Session["Ad_Role"] != null)
            //{
            string id = Session["Ad_Id"].ToString().Trim();
            var data = db.Users.Find(Session["Ad_Id"]);
            return PartialView("_Header", data);
            //}
            //else {
            //    return Redirect("AdminIT/Account/Login");
            //}
        }

        [ChildActionOnly]
        public ActionResult LoadMenu()
        {
            //if (Session["Ad_Role"] != null)
            //{
            string id = Session["Ad_Role"].ToString().Trim();
            var data = db.RoleFunctions.Where(e => e.RoleId.Trim().Equals(id)).ToList();
                return PartialView("_Menu", data);
            //}
            //else {
            //    return Redirect("AdminIT/Account/Login");
            //}
        }

        public ActionResult LoadTreeFunction()
        {
            var data = db.Categories.Where(e =>e.Type==4 &&  e.Publish == 1).ToList();

            return PartialView("_TreeCategory", data);
        }

        public ActionResult LoadTreeCategory()
        {
            var data = db.Categories.Where(e => e.Type != 4 && e.Publish == 1).ToList();

            return PartialView("_TreeCategory",data);
        }

        //public ActionResult LoadTreeMenuCategory(string typeMenu)
        //{
        //    var data = from c in db.Categories
        //               from m in db.Menus.Where(m => m.TypeMenuId == typeMenu)
        //               where c.Id == m.CategoryId && c.Type != 4
        //               select c;
        //    return PartialView("_TreeMenuCategory", data);
        //}

        public ActionResult LoadTreeMenuFunction(string typeMenu)
        {
            var data = from c in db.Categories
                       from m in db.RoleFunctions.Where(m => m.RoleId == typeMenu)
                       where c.Id == m.CategoryId && c.Type == 4
                       select c;
            return PartialView("_TreeMenuCategory", data);
        }

        //public ActionResult LoadTreeMenuNoCategory(string typeMenu)
        //{
        //    var data = from c in db.Categories.Where(e=>e.Type!=4)
        //               where !db.Menus.Any(m => m.CategoryId == c.Id && m.TypeMenuId == typeMenu)
        //               select c;
        //    return PartialView("_TreeMenuNoCategory", data);
        //}

        public ActionResult LoadTreeMenuNoFunction(string typeMenu)
        {
            var data = from c in db.Categories.Where(e=>e.Type==4)
                       where !db.RoleFunctions.Any(m => m.CategoryId == c.Id && m.RoleId == typeMenu)
                       select c;
            return PartialView("_TreeMenuNoCategory", data);
        }

        //public ActionResult LoadTreeMenu(string id)
        //{
        //    var data = db.TypeMenus.Find(id);
        //    return PartialView("_TreeMenu", data);
        //}

        public ActionResult LoadTreeMenuFunc(string id)
        {
            var data = db.Roles.Find(id);
            return PartialView("_TreeMenuFunc", data);
        }

	}
}