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
    [Authorize(Roles = "Administrator")]
    public class RolesController : Controller
    {

        lCMSData db = new lCMSData();
        //
        // GET: /AdminIT/Roles/
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

            var model = db.Roles;
            return View(model);
        }

        //
        // GET: /AdminIT/Roles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AdminIT/Roles/Create
        public ActionResult Create()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            return View();
        }

        //
        // POST: /AdminIT/Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, Role model)
        {
            try
            {
                string output = collection["output2"];

                output = @"{""data"":" + output + "}";

                var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(output);

                Role tm = new Role();
                string Id = Public.GetID2();
                tm.Id = Id;
                tm.Name = collection["Name"];
                db.Roles.Add(tm);
                db.SaveChanges();
                FunctionItem(jsonResult["data"], Id, "0");

                return RedirectToAction("List");
            }
            catch
            {
                TempData["error"] = "Thêm lỗi.";
                return RedirectToAction("List");
            }
        }

        private void FunctionItem(dynamic jsonResult, string roleId, string functionParent)
        {
            for (int i = 0; i < jsonResult.Count; i++)
            {
                string Id = Public.GetID2();
                int id = jsonResult[i]["id"];
                if (id != 0)
                {
                    RoleFunction model = new RoleFunction();
                    model.Id = Id;
                    model.CategoryId = id;
                    model.RoleId = roleId;
                    model.Location = (i + 1);
                    model.FunctionParent = functionParent;
                    model.DatePublish = DateTime.Now;
                    model.UserId = int.Parse(Session["Ad_Id"].ToString());
                    db.RoleFunctions.Add(model);
                }
                dynamic children = jsonResult[i]["children"];
                if (children != null)
                {
                    FunctionItem(children, roleId, Id);
                }
            }
            db.SaveChanges();
            TempData["success"] = "Thêm thành công.";
        }

        //
        // GET: /AdminIT/Roles/Edit/5
        public ActionResult Edit(string id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            var data = db.Roles.Find(id);

            return View(data);
        }

        //
        // POST: /AdminIT/Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection, string id)
        {
            var tm = db.Roles.Find(id);
            string output = collection["output2"];

            output = @"{""data"":" + output + "}";

            var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(output);

            var menu = db.RoleFunctions.Where(e => e.RoleId == id);
            foreach (var m in menu)
            {
                db.RoleFunctions.Remove(m);
            }
            db.SaveChanges();

            FunctionItem(jsonResult["data"], id, "0");

            return RedirectToAction("List");
        }

        //
        // GET: /AdminIT/Roles/Delete/5
        public ActionResult Delete(string id)
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            ViewBag.Name = Session["Ad_TenDangNhap"];

            var role = db.Roles.Find(id);
            ViewBag.Meg = role.Name;
            return View();
        }

        //
        // POST: /AdminIT/Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(string id,FormCollection collection)
        {
            var data = db.Roles.Find(id);
            var rfunc = db.RoleFunctions.Where(e => e.RoleId == id);
            foreach(var rf in rfunc){
                db.RoleFunctions.Remove(rf);
            }
            db.Roles.Remove(data);
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
