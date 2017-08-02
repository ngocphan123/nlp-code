using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using Console = System.Console;
using java.util;
using java.io;
using StudyDoIT.Models.NLP;
using StudyDoIT.Models.Common;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace StudyDoIT.Areas.AdminIT.Controllers
{
    public class ToolsTestController : Controller
    {

        lCMSData db = new lCMSData();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SlipString()
        {
            return View();
        }

        public ActionResult PosTagging()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            return View();
        }

        [HttpPost]
        public ActionResult PosTagging(FormCollection collection)
        {
            string text = collection["Description"];
            string urlRoot = System.IO.Path.Combine(Server.MapPath("~/Uploads/english-left3words"), "english-left3words-distsim.tagger");
            var tagger = new MaxentTagger(urlRoot);
            string str = "";

            var sentences = MaxentTagger.tokenizeText(new StringReader(text)).toArray();

            foreach (ArrayList sentence in sentences)
            {
                try
                {
                    var taggedSentence = tagger.tagSentence(sentence);
                    str += taggedSentence.ToString() + ".";
                    //string[] str1 = taggedSentence.ToString().Split(',', '[', ']');
                }
                catch { }
            }
            ViewBag.Resulft = text;
            //var data=db.V
            //return PartialView("_ResulftPosTag", null);
            //return PartialView("_ListVocabulary", data5);
            //var data5 = db.Vocabularies.ToList();
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