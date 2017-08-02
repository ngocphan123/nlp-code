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
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace StudyDoIT.Areas.AdminIT.Controllers
{
    public class TranningResuftController : Controller
    {
        lCMSData db = new lCMSData();
        //
        // GET: /AdminIT/TranningResuft/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /AdminIT/TranningResuft/Details/5
        //public ActionResult LoadResuftOther()
        //{
        //    var data = db.SeKeyWords.ToList();
        //    return PartialView("_ResuftAddLabel", data);
        //}

        public ActionResult ResuftAddLabel()
        {
            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name");
            var data2 = db.SeKeyWords.Where(e => e.Sentens.Comment.GroupCommentId == "").ToList();
            return View(data2);
        }

        [HttpPost]
        public ActionResult LoadResuftAddLabel(FormCollection collection)
        {
            string idgc = collection["GroupComentId"];

            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name", idgc);

            var data2 = db.SeKeyWords.Where(e => e.Sentens.Comment.GroupCommentId == idgc).ToList();
            // retrive the data from table  
            //var personlist = db.People.ToList();
            // Pass the "personlist" object for conversion object to JSON string  
            //string jsondata = new JavaScriptSerializer().Serialize(data);
            //string path = Server.MapPath("~/App_Data/");
            // Write that JSON to txt file,  
            //System.IO.File.WriteAllText(path + "output.json", jsondata); 
            //TempData["info"] = "Sửa thành công.";

            //return RedirectToAction("ResuftAddLabel");

            return View("ResuftAddLabel", data2);
        }

        public ActionResult ResuftLogAddLabel()
        {
            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult LoadResuftLogAddLabel(FormCollection collection)
        {
            string idgc = collection["GroupComentId"];

            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name", idgc);

            var data2 = db.LogLabels.Where(e => e.GroupCommentId == idgc).ToList();

            return View("ResuftLogAddLabel", data2);
        }

        public ActionResult ResuftLogAddKeyWords()
        {
            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult LoadResuftLogAddKeyWords(FormCollection collection)
        {
            string idgc = collection["GroupComentId"];

            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name", idgc);

            var data2 = db.LogAddKeyWords.Where(e => e.GroupCommentId == idgc).ToList();

            return View("ResuftLogAddKeyWords", data2);
        }

        public ActionResult ResuftAddKeyWords()
        {
            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name");

            IEnumerable<GroupWord> data3 = (IEnumerable<GroupWord>)db.GroupWords.ToList();
            ViewBag.GroupWord = new SelectList(data3, "Id", "Word");

            return View();
        }

        [HttpPost]
        public ActionResult LoadResuftAddKeyWords(FormCollection collection)
        {
            string idgc = collection["GroupComentId"];
            string idgw = collection["GroupWordId"];

            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name", idgc);

            IEnumerable<GroupWord> data3 = (IEnumerable<GroupWord>)db.GroupWords.ToList();
            ViewBag.GroupWord = new SelectList(data3, "Id", "Word", idgw);

            var data2 = db.KeyWords.Where(e => e.GroupCommentId == idgc && e.GroupWordId==idgw).ToList();

            return View("ResuftAddKeyWords", data2);
        }

        public ActionResult ResuftKeyWordsCount()
        {
            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name");

            IEnumerable<GroupWord> data3 = (IEnumerable<GroupWord>)db.GroupWords.ToList();
            ViewBag.GroupWord = new SelectList(data3, "Id", "Word");

            return View();
        }

        [HttpPost]
        public ActionResult LoadResuftKeyWordsCount(FormCollection collection)
        {
            string idgc = collection["GroupComentId"];
            string idgw = collection["GroupWordId"];

            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name", idgc);

            IEnumerable<GroupWord> data3 = (IEnumerable<GroupWord>)db.GroupWords.ToList();
            ViewBag.GroupWord = new SelectList(data3, "Id", "Word", idgw);

            var data2 = db.KeywordsCounts.Where(e => e.GroupCommentId == idgc && e.GroupKeyWordId == idgw).ToList();

            return View("ResuftKeyWordsCount", data2);
        }

        public ActionResult MatrixCountWords()
        {
            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name");

            IEnumerable<GroupWord> data3 = (IEnumerable<GroupWord>)db.GroupWords.ToList();
            ViewBag.GroupWord = new SelectList(data3, "Id", "Word");

            return View();
        }

        [HttpPost]
        public ActionResult MatrixCountWords(FormCollection collection)
        {
            string idgc = collection["GroupComentId"];
            string idgw = collection["GroupWordId"];

            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name", idgc);

            IEnumerable<GroupWord> data3 = (IEnumerable<GroupWord>)db.GroupWords.ToList();
            ViewBag.GroupWord = new SelectList(data3, "Id", "Word", idgw);

            var data2 = db.KeyWords.Where(e => e.GroupCommentId == idgc && e.GroupWordId == idgw && e.TypeWord==1).ToList();

            return View("MatrixCountWords", data2);
        }

        public ActionResult MatrixProbabilityWords()
        {
            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name");

            IEnumerable<GroupWord> data3 = (IEnumerable<GroupWord>)db.GroupWords.ToList();
            ViewBag.GroupWord = new SelectList(data3, "Id", "Word");

            return View();
        }

        [HttpPost]
        public ActionResult MatrixProbabilityWords(FormCollection collection)
        {
            string idgc = collection["GroupComentId"];
            string idgw = collection["GroupWordId"];

            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name", idgc);

            IEnumerable<GroupWord> data3 = (IEnumerable<GroupWord>)db.GroupWords.ToList();
            ViewBag.GroupWord = new SelectList(data3, "Id", "Word", idgw);

            var data2 = db.KeyWords.Where(e => e.GroupCommentId == idgc && e.GroupWordId == idgw && e.TypeWord == 1).ToList();

            return View("MatrixProbabilityWords", data2);
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
