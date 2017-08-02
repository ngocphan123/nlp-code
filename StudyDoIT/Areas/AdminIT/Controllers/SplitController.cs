using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyDoIT.Models.NLP;
using StudyDoIT.Models.Common;

namespace StudyDoIT.Areas.AdminIT.Controllers
{
    
    public class SplitController : Controller
    {
        lCMSData db = new lCMSData();
        public ActionResult Index()
        {
            var cm = db.Comments.ToList();
            try
            {
                foreach (var item in cm)
                {
                    try
                    {
                        var sen =db.Sentenses.Where(e=>e.CommentId==item.Id).ToList();
                        if (sen.Count <= 0)
                        {
                            string[] str = item.Comment1.Split('.', '!', '?');

                            foreach (var s in str)
                            {
                                try
                                {
                                    if (s.Trim() != "" && s.Length > 2)
                                    {
                                        string ids = Public.GetID();
                                        while (db.Sentenses.Where(e => e.Id == ids).Count() > 0)
                                        {
                                            ids = Public.GetID();
                                        }
                                        Sentens se = new Sentens();
                                        se.Id = ids;
                                        se.ContentReview = s;
                                        se.CommentId = item.Id;
                                        db.Sentenses.Add(se);
                                        db.SaveChanges();
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }
            
            return View();
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
