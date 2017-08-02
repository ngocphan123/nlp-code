using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Security;
using StudyDoIT.Models.NLP;
using StudyDoIT.Models.Common;
using System.IO;

namespace StudyDoIT.Areas.AdminIT.Controllers
{

    public class AccountController : Controller
    {
        lCMSData db = new lCMSData();

        HttpCookie ckTenDangNhap = null;

        HttpCookie ckMatKhau = null;
        //
        // GET: /AdminIT/Account/
        public ActionResult Index()
        {
            return RedirectToAction("Index","Home",null);
        }


        public ActionResult Login()
        {
            ckMatKhau = Request.Cookies["Ad_MatKhau"];
            ckTenDangNhap = Request.Cookies["Ad_TenDangNhap"];
            if (ckTenDangNhap != null)
            {
                if (CheckLogin(ckTenDangNhap.Value, ckMatKhau.Value, true))
                {
                    FormsAuthentication.SetAuthCookie(ckTenDangNhap.Value, false);
                    if (Session["current_url"] != null)
                        return Redirect(Session["current_url"].ToString());
                    return RedirectToAction("Index", "Home", null);
                }
            }
            return View();
        }

        bool CheckLogin(string TenDangNhap, string MatKhau, bool remember = false)
        {
            if (TenDangNhap != null && MatKhau != null)
            {
                string MatKhauMd5 = Public.SHA256(MatKhau);

                var users = (from p in db.Users
                             where p.UserName == TenDangNhap && p.PasswordHash == MatKhauMd5
                             select p
                            );

                if (users.Count() == 0)
                {
                    ModelState.AddModelError("Error", "Tên đăng nhập hoặc mật khẩu không đúng.");
                    return false;
                }

                if (users.First().Lock == true)
                {
                    ModelState.AddModelError("Error", "Tài khoản của bạn bị khóa.");
                    return false;
                }

                if (users.First().EmailConfirmaed == false)
                {
                    ModelState.AddModelError("Error", "Tài khoản của bạn chưa được kích hoạt.");
                    return false;
                }

                if (remember)
                {
                    // check if cookie exists and if yes update
                    HttpCookie ckTenDangNhap = null;

                    HttpCookie ckMatKhau = null;

                    //create a cookie
                    ckTenDangNhap = new HttpCookie("Ad_TenDangNhap");
                    ckMatKhau = new HttpCookie("Ad_MatKhau");

                    ckTenDangNhap.Value = TenDangNhap;
                    ckMatKhau.Value = MatKhau;


                    ckTenDangNhap.Expires = DateTime.Today.AddDays(15);
                    ckMatKhau.Expires = DateTime.Today.AddDays(15);

                    Response.Cookies.Add(ckTenDangNhap);
                    Response.Cookies.Add(ckMatKhau);
                }

                Session["Ad_TenDangNhap"] = users.First().UserName;
                Session["Ad_Role"] = users.First().RoleId;
                Session["Ad_Id"] = users.First().Id;
                return true;
            }
            return false;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection collection, User model)
        {
            string user = collection["UserName"];
            string pass = collection["PassWord"];
            string remember = collection["RememberMe"];
            bool rb = false;
            if (remember == "false")
            {
                rb = false;
            }
            else
            {
                rb = true;
            }

            if (user != null && pass != null)
            {
                if (CheckLogin(user, pass, rb))
                {
                    if (Session["current_url"] != null)
                        return Redirect(Session["current_url"].ToString());
                }
                else
                {
                    return RedirectToAction("Login", "Account", null);
                }
            }
            return RedirectToAction("Index", "Home", null);
        }

        public ActionResult Logout()
        {
            ckTenDangNhap = new HttpCookie("Ad_TenDangNhap");
            ckMatKhau = new HttpCookie("Ad_MatKhau");

            ckTenDangNhap.Value = null;
            ckMatKhau.Value = null;

            ckTenDangNhap.Expires = DateTime.Today.AddDays(-1);
            ckMatKhau.Expires = DateTime.Today.AddDays(-1);

            Response.Cookies.Add(ckTenDangNhap);
            Response.Cookies.Add(ckMatKhau);

            Session["Ad_TenDangNhap"] = null;
            Session["Ad_MaQuyen"] = null;
            return RedirectToAction("Login", "Account", null);
        }

        public ActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult Register(FormCollection colection, SentContact model)
        {
            //if (ModelState.IsValid)
            //{
            if (colection["AgreeTerms"] == "on")
            {
                var sm = db.MailServers.Find(4);
                string userName = sm.UserName;
                string password = sm.Password;
                string smtpHost = sm.Host;
                int smtpPort = int.Parse(sm.Port);
                //thong tin email được gởi
                string toEmail = colection["Email"];//các liên hệ sẽ gởi về email này
                string username = colection["UserName"];

                var user = db.Users.Where(e => e.Email == toEmail || e.UserName==username);

                if (user.Count() <= 0)
                {
                    string code = "";
                    do
                    {
                        code = Public.RandomStringNumber(8);
                    } while (db.Users.Where(e => e.SecurityStamp == code).Count() > 0);

                    string link = "http://myphamtamduc.com/AdminIT/Account/UserAuthentication/"+username;
                     
                    User us = new User();
                    us.FullName = username;
                    us.UserName = username;
                    us.Email = toEmail;
                    us.EmailConfirmaed = false;
                    us.Lock = true;
                    us.PasswordHash = Public.SHA256(colection["PassWord"]);
                    us.PhoneNumber = "";
                    us.PhoneNumberConfirmed = false;
                    us.Avata = "/Test/dragonviet.png";
                    us.RoleId = "151225031247673";
                    us.SecurityStamp = code;
                    us.Sex = "";
                    us.LockoutEnabled = false;
                    us.LockoutEndDateUtc = DateTime.Now;
                    us.Birthday = DateTime.Now;
                    us.Address = "";

                    model.Content = "Mã kích hoạt tài khoản của bạn là: " + code +" </br> Bạn vào đường dẫn sau để kích hoạt tài khoản: "+link;
                    model.Subject = "Thông tin đăng ký tài khoản";
                    model.ToEmail = toEmail;

                    string body = string.Format("Chúc mừng tài khoản:{0} đã đăng ký thành công! </br> {1}", username, model.Content);

                    EmailService email = new EmailService();

                    bool result = email.Send(userName, password, smtpHost, smtpPort, model.ToEmail, model.Subject, body);

                    if (result)
                    {                      
                        db.Users.Add(us);
                        db.SaveChanges();
                        TempData["success"] = "Chúc mừng bạn đã tạo tài khoản thành công!, Bạn vào email để nhận mã kích hoạt!";
                        return RedirectToAction("UserAuthentication", "Account", new { id=username});
                    }
                }
                else
                {
                    TempData["error"] = "Email hoặc tài khoản đã được sử dụng, vui lòng thử lại.";
                    //ModelState.AddModelError("", "Đăng ký thất bại, vui lòng thử lại");
                }
            }
            return View(new User());
        }

        public ActionResult RecoverPassword()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult RecoverPassword(FormCollection colection, SentContact model)
        {
            //if (ModelState.IsValid)
            //{
            var sm = db.MailServers.Find(4);
            string userName = sm.UserName;
            string password = sm.Password;
            string smtpHost = sm.Host;
            int smtpPort = int.Parse(sm.Port);
            //thong tin email được gởi
            string toEmail = colection["Email"];//các liên hệ sẽ gởi về email này

            var user = db.Users.Where(e => e.Email == toEmail && e.Lock == false);

            if (user.Count() > 0)
            {
                string rdPass=Public.RandomStringNumber(8);
                model.Content = "Mật khẩu mới của bạn là: " + rdPass;
                model.Subject = "Lấy lại mật khẩu";
                model.ToEmail = toEmail;
         
                string body = string.Format("Thông tin tài khoản:{0} </br>" +
                                            " {1}", user.First().UserName, model.Content);

                EmailService email = new EmailService();

                bool result = email.Send(userName, password, smtpHost, smtpPort, model.ToEmail, model.Subject, body);

                if (result)
                {
                    TempData["success"] = "Thông tin lấy lại mật khẩu đã đươc gủi đi thành công, Bạn vào email nhận mật khẩu mới";

                    var users = db.Users.Where(e=>e.Email==toEmail).First();
                    users.PasswordHash = Public.SHA256(rdPass);
                    db.Entry(users).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Login", "Account", null);
                    // ModelState.AddModelError("", "Bạn đã đăng ký nhận tin khuyến mại thành công");
                }
            }
            else
            {
                TempData["error"] = "Email không tồn tại, vui lòng thử lại.";
                //ModelState.AddModelError("", "Đăng ký thất bại, vui lòng thử lại");
            }
            return View();
        }

        public ActionResult UserAuthentication(string id)
        {
            var user = db.Users.Where(e=>e.UserName==id).First();
            return View(user);
        }

        [HttpPost]
        public ActionResult UserAuthentication(FormCollection colection, string id, SentContact model)
        {
            var user = db.Users.Where(e => e.UserName == id).First();
            if (user.SecurityStamp.Trim() == colection["Code"].ToString().Trim())
            {
                var sm = db.MailServers.Find(4);
                string userName = sm.UserName;
                string password = sm.Password;
                string smtpHost = sm.Host;
                int smtpPort = int.Parse(sm.Port);
                //thong tin email được gởi
                    user.EmailConfirmaed = true;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    string link = "http://myphamtamduc.com/AdminIT/Account/Login";

                    model.Content = "Chúc mừng bạn đã kích hoạt tài khoản thành công, bạn có thể vào đường dẫn sau để tiến hành đăng nhập: " + link;
                    model.Subject = "Kích hoạt tài khoản";
                    model.ToEmail = user.Email;

                    string body = string.Format("Thông tin tài khoản:{0} </br>" +
                                                " {1}", user.UserName, model.Content);

                    EmailService email = new EmailService();

                    bool result = email.Send(userName, password, smtpHost, smtpPort, model.ToEmail, model.Subject, body);

                    
                    if (result)
                    {
                        TempData["success"] = "Chúc mừng bạn đã kích hoạt tài khoản thành công!";
                        return RedirectToAction("Login", "Account", null);
                    }
            }
            return View(user);
        }

        public ActionResult ChangePass(int id)
        {
            var user = db.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult ChangePass(FormCollection colection, int id, SentContact model)
        {
            var user = db.Users.Find(id);

            //if (ModelState.IsValid)
            //{
            var sm = db.MailServers.Find(4);
            string userName = sm.UserName;
            string password = sm.Password;
            string smtpHost = sm.Host;
            int smtpPort = int.Parse(sm.Port);
            //thong tin email được gởi
            string pass1 = colection["PassWord"];
            string pass2 = colection["PassWordConfirm"];
            if (pass1 == pass2)
            {
                user.PasswordHash = Public.SHA256(pass1);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                model.Content = "Mật khẩu của bạn đã được thay đổi, nếu không phải là bạn thay đổi mật khẩu, liên hệ với quản trị viên theo Email: " + userName;
                model.Subject = "Đổi mật khẩu";
                model.ToEmail = user.Email;

                string body = string.Format("Thông tin tài khoản:{0} </br>" +
                                            " {1}", user.UserName, model.Content);

                EmailService email = new EmailService();

                bool result = email.Send(userName, password, smtpHost, smtpPort, model.ToEmail, model.Subject, body);

                return RedirectToAction("Login", "Account", null);
                //if (result)
                //{
                //    TempData["success"] = "Thông tin đổi mật khẩu đã đươc gủi đi thành công, Bạn vào email nhận mật khẩu mới";
                //    // ModelState.AddModelError("", "Bạn đã đăng ký nhận tin khuyến mại thành công");
                //}
            }
            else
            {
                TempData["error"] = "Mật khẩu nhập lại không khớp!";
            }
            return View(user);
        }

        public ActionResult LockScreem()
        {
            return View();
        }

        public ActionResult InfoUser()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            string username = Session["Ad_TenDangNhap"].ToString();
            var user = db.Users.Where(e => e.UserName.Trim().ToUpper().Equals(username.Trim().ToUpper())).First();
            return View(user);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult InfoUser(FormCollection collection)
        {
            //Session["current_url"] = Request.Url.AbsoluteUri;

            //if (Session["Ad_TenDangNhap"] == null)
            //    return RedirectToAction("Login", "Account", null);

            string username = Session["Ad_TenDangNhap"].ToString();
            var model = db.Users.Where(e => e.UserName.Trim().ToUpper().Equals(username.Trim().ToUpper())).First();

            model.FullName = collection["FullName"];
            model.Sex = collection["Sex"];
            model.Address = collection["Address"];
            model.PhoneNumber = collection["PhoneNumber"];
            model.Birthday = DateTime.Parse(collection["Birthday"]);
            model.Jobs = collection["Jobs"];

            string[] img = collection["Avata"].ToString().Split('/');
            if (img.Count() > 1)
            {
                if (img[1] != "Uploads")
                {
                    model.Avata = collection["Avata"];
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

            //thêm Cover
            string fileName2 = "";
            string path = "";
            var files2 = Request.Files["imgCover"];

            if (files2 != null && files2.ContentLength > 0)
            {
                try
                {
                    if (Request.Browser.Browser == "IE")
                        fileName2 = Path.GetFileName(files2.FileName);
                    else
                        fileName2 = files2.FileName;

                    path = Path.Combine(Server.MapPath("~/Uploads/files"), fileName2);
                    files2.SaveAs(path);
                    model.Cover = fileName2;
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Thêm file đính kèm lỗi.";
                    return RedirectToAction("InfoUser");
                }
            }
            else
            {
                if (model.Cover == "" || model.Cover==null)
                {
                    TempData["info"] = "Chưa chọn file đính kèm.";
                    return RedirectToAction("InfoUser");
                }
            }
            
            //thêm CV
            string fileName3 = "";
            string path3 = "";
            var files3 = Request.Files["fileAttactment"];
            if (files3 != null && files3.ContentLength > 0)
            {
                try
                {
                    if (Request.Browser.Browser == "IE")
                        fileName3 = Path.GetFileName(files3.FileName);
                    else
                        fileName3 = files3.FileName;

                    path3 = Path.Combine(Server.MapPath("~/Uploads/files"), fileName3);
                    files3.SaveAs(path3);
                    model.Attactment = fileName3;
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Thêm file đính kèm lỗi.";
                    return RedirectToAction("InfoUser");
                }
            }
            else
            {
                if (model.Attactment == "" || model.Attactment == null)
                {
                    TempData["info"] = "Chưa chọn file đính kèm.";
                    return RedirectToAction("InfoUser");
                }
            }                      

            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("InfoUser");
  
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