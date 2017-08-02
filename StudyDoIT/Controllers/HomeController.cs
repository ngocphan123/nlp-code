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

namespace StudyDoIT.Controllers
{
    public class HomeController : Controller
    {
        lCMSData db = new lCMSData();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadStart(int id)
        {
            var data = db.Users.Find(id);
            return PartialView("_Start", data);
        }

        public ActionResult LoadResumeBoxSkills(int id)
        {
            var data = db.SkillsAbilities.Where(e=>e.UserId==id && e.Type==1).ToList();
            return PartialView("_ResumeBoxSkills", data);
        }

        public ActionResult LoadResumeBoxSpecilities(int id)
        {
            var data = db.MySpecialities.Where(e => e.UserId == id).ToList();
            return PartialView("_ResumeBoxSpecilities", data);
        }

        public ActionResult LoadResumeBoxEducationJobs(int id)
        {
            var data = db.EducationJobs.Where(e => e.UserId == id).ToList();
            return PartialView("_ResumeBoxEducationJobs", data);
        }

        public ActionResult LoadResumeBoxLanguages(int id)
        {
            var data = db.SkillsAbilities.Where(e => e.UserId == id && e.Type==2).ToList();
            return PartialView("_ResumeBoxLanguages", data);
        }

        public ActionResult LoadResumeBoxHobbies(int id)
        {
            var data = db.HobbiesInterests.Where(e => e.UserId == id).ToList();
            return PartialView("_ResumeBoxHobbies", data);
        }

        public ActionResult LoadPost(int id)
        {
            var data = db.Projects.Where(e => e.UserId == id).ToList();
            return PartialView("_Posts", data);
        }

        public ActionResult LoadContact(int id)
        {
            var data = db.Users.Find(id);
            return PartialView("_Contact", data);
        }

        public ActionResult LoadMessanger(int id)
        {
            var data = db.Users.Find(id);
            return PartialView("_Messanger", data);
        }

        public ActionResult LoadMobileNavigation(string id)
        {
            var data = db.Menus.Where(e=>e.TypeMenuId==id);
            return PartialView("_MobileNavigation", data);
        }



        //[HttpPost]
        public ActionResult Sent(string name, string aemail, string message="")
        {
            //try
            //{
                Contact model = new Contact();
                model.Id = Public.GetID2();
                model.Name = name;
                model.Email = aemail;
                model.Contents = message;
                model.DateSend = DateTime.Now;
                model.UserId = 1;

                var ms = db.MailServers.Find(4);
                string userName = ms.UserName;
                string password = ms.Password;
                string smtpHost = ms.Host;//nếu dùng gmail thì:smtp.gmail.com
                int smtpPort = int.Parse(ms.Port);//toi dùng port của gmail

                SentContact sc = new SentContact();
                sc.ToEmail = model.Email;//các liên hệ sẽ gởi về email này
                sc.Content = "Chúc mừng bạn " + model.Name + " đã liên hệ thành công!";
                sc.Subject = "Liên hệ";
                string body = string.Format(sc.Content);

                EmailService email = new EmailService();

                bool result = email.Send(userName, password, smtpHost, smtpPort, sc.ToEmail, sc.Subject, body);

                if (result)
                {
                    db.Contacts.Add(model);
                    db.SaveChanges();
                    TempData["success"] = "Liên hệ thành công!";
                }
                else
                {
                    //TempData["error"] = "Liên hệ lỗi, mời bạn thực hiện lại!";
                    int.Parse("AAA");
                }

                return PartialView("_Start");
            //}
            //catch
            //{
            //    TempData["error"] = "Liên hệ lỗi, mời bạn thực hiện lại!";
            //    return PartialView("_Start");
            //}
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
