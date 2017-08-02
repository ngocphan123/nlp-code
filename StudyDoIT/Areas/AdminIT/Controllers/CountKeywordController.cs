using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyDoIT.Models.NLP;
using StudyDoIT.Models.Common;
using System.Text.RegularExpressions;
using System.Data.Entity;


namespace StudyDoIT.Areas.AdminIT.Controllers
{
    public class CountKeywordController : Controller
    {
        lCMSData db = new lCMSData();
        //
        // GET: /AdminIT/CountKeyword/
        public ActionResult Index()
        {
            var gc = db.GroupComents.Where(e => e.Id == "170110042257128").ToList();
            var kw = db.KeyWords.ToList();
            foreach (var itemGC in gc)
            {
                var cm = db.Comments.Where(e => e.GroupCommentId == itemGC.Id).ToList();

                foreach (var itemKW in kw)
                {
                    string sN = "\\b" + Convert.ToString(itemKW.Word) + "\\b";
                    try
                    {
                        var item = db.CountKeywords.Where(e => e.KeyWordId == itemKW.Id && e.GroupCommentId == itemGC.Id).ToList();
                        if (item.Count <= 0)
                        {
                            int count = 0;
                            foreach (var itemCM in cm)
                            {                             
                                Regex thegex = new Regex(sN.ToLower());
                                MatchCollection theMatches = thegex.Matches(itemCM.Comment1);
                                count += theMatches.Count;
                            }
                            CountKeyword ck = new CountKeyword();
                            ck.Id = Public.GetID();
                            ck.KeyWordId = itemKW.Id;
                            ck.GroupCommentId = itemGC.Id;
                            ck.Count = count;
                            db.CountKeywords.Add(ck);
                            db.SaveChanges();
                        }
                    }
                    catch { }
                }
            }

            var data=db.CountKeywords.GroupBy(l => l.KeyWord.Word).Select(cl => new ItemCountWords
            {
                GroupWord= cl.FirstOrDefault().KeyWord.GroupWord.Word,
                Word = cl.FirstOrDefault().KeyWord.Word,
                Count = cl.Sum(c => c.Count).Value,
                Type= cl.FirstOrDefault().KeyWord.Type.ToString()
            }).ToList();
            return View(data);

        }


        public ActionResult CountVocabulary()
        {
            var vc = db.Vocabularies.ToList();
            var cm = db.Comments.ToList();
            foreach (var itemV in vc)
            {
                int count = 0;
                foreach (var itemCM in cm)
                {
                    string sN = "\\b" + Convert.ToString(itemV.Word.Trim().ToLower()) + "\\b";
                    Regex thegex = new Regex(sN.ToLower());
                    MatchCollection theMatches = thegex.Matches(itemCM.Comment1.Trim().ToLower());
                    count += theMatches.Count;
                }

                var uv = db.Vocabularies.Find(itemV.Id);
                uv.Counts = count;
                db.Entry(uv).State = EntityState.Modified;
                db.SaveChanges();                               
            }

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
