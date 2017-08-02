using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyDoIT.Models.NLP;
using StudyDoIT.Models.Common;
using System.Text.RegularExpressions;
using System.Data.Entity;
using edu.stanford.nlp.tagger.maxent;
using java.io;

namespace StudyDoIT.Areas.AdminIT.Controllers
{
    public class ListVocabulary
    {
        public string Id { get; set; }
        public string GroupWords { get; set; }
        public string Words { get; set; }
        public double? C1 { get; set; }
        public double? C2 { get; set; }
        public double? C3 { get; set; }
        public double? C4 { get; set; }
        public double? S1 { get; set; }
        public double? S2 { get; set; }
        public double? S3 { get; set; }
        public double? C { get; set; }
        public double? X2 { get; set; }
    }

    public class ListGroupKeyWord
    {
        public string Id { get; set; }
        public string Words { get; set; }
        public int? Count { get; set; }
    }

    public class ListCountKeyWord
    {
        public string Id { get; set; }
        public string Words { get; set; }
        public string GroupWords { get; set; }
        public int? Count { get; set; }
    }

    public class AddLabelsController : Controller
    {
        lCMSData db = new lCMSData();
        //
        // GET: /AdminIT/AddLabels/
        public ActionResult Index()
        {
            var GC = db.GroupComents.ToList();
            foreach (var itemGC in GC)
            {
                int kc = 0;
                while (kc < 3)
                {
                    try
                    {
                        var S1 = db.Sentenses.Where(e => e.Comment.GroupCommentId == itemGC.Id).ToList();
                        #region Gán nhãn câu
                        foreach (var itemS in S1)
                        {
                            var gW = db.GroupWords.ToList();
                            List<ListCountKeyWord> countKW = new List<ListCountKeyWord>();
                            List<ListGroupKeyWord> countGW = new List<ListGroupKeyWord>();
                            #region So khớp từ khóa
                            foreach (var itemGW in gW)
                            {
                                var w = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id).ToList();
                                int www = 0;
                                foreach (var itemW in w)
                                {
                                    try
                                    {
                                        string sN = "\\b" + Convert.ToString(itemW.Word) + "\\b";
                                        Regex thegex = new Regex(sN.Trim().ToLower());
                                        MatchCollection theMatches = thegex.Matches(itemS.ContentReview.Trim().ToLower());
                                        www += theMatches.Count;
                                        if (theMatches.Count > 0)
                                        {
                                            countKW.Add(new ListCountKeyWord { Count = theMatches.Count, Id = itemW.Id, Words = itemW.Word, GroupWords = itemGW.Word });
                                        }
                                    }
                                    catch { }
                                }
                                countGW.Add(new ListGroupKeyWord { Id = itemGW.Id, Count = www, Words=itemGW.Word });
                            }
                            #endregion
                            #region Sắp xếp
                            try
                            {
                                if (countGW.Count > 0)
                                {
                                    //Sắp xếp các countW
                                    for (int i = 0; i < countGW.Count - 1; i++)
                                    {
                                        for (int j = i + 1; j < countGW.Count; j++)
                                        {
                                            if (countGW[i].Count < countGW[j].Count)
                                            {
                                                ListGroupKeyWord tg = countGW[i];
                                                countGW[i] = countGW[j];
                                                countGW[j] = tg;
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }
                            #endregion

                            #region Log gán nhãn
                            try
                            {
                                LogLabel log = new LogLabel();
                                string logid = Public.GetID();
                                while (db.LogLabels.Where(e => e.Id == logid).ToList().Count > 0)
                                {
                                    logid = Public.GetID();
                                }
                                log.Id = logid;
                                log.ReviewContent = itemS.ContentReview;
                                string logcount = "";
                                string groupword = "";
                                foreach (var kws in countKW)
                                {
                                    logcount += kws.Words + "/" + kws.Count + " | ";
                                }

                                foreach (var gkws in countGW)
                                {
                                    if (gkws.Count > 0)
                                    {
                                        groupword += gkws.Words + "/" + gkws.Count + " | ";
                                    }
                                }
                                log.LogCounts = logcount;
                                log.Steps += "Chạy lần " + kc;
                                log.GroupKeywords = groupword;
                                db.LogLabels.Add(log);
                                db.SaveChanges();
                            }
                            catch { }

                            #endregion

                            #region Gán nhãn từ khóa
                            try
                            {                                
                                if (countGW.Count > 0)
                                {
                                    if (countGW[0].Count > 0)
                                    {
                                        //Xóa câu đã được gán nhãn trc đó để gán nhãn lại
                                        var skw3 = db.SeKeyWords.Where(e => e.SeId == itemS.Id).ToList();
                                        foreach (var itemSKW3 in skw3)
                                        {
                                            var skw4 = db.SeKeyWords.Find(itemSKW3.Id);
                                            db.SeKeyWords.Remove(skw4);
                                            db.SaveChanges();
                                        }

                                        for (int i = 0; i < countGW.Count; i++)
                                        {
                                            if (countGW[i].Count == countGW[0].Count)
                                            {
                                                string skwi = countGW[i].Id;
                                                if (db.SeKeyWords.Where(e => e.KeyWordId == skwi && e.SeId == itemS.Id).ToList().Count <= 0)
                                                {
                                                    SeKeyWord skw2 = new SeKeyWord();
                                                    string skwid = Public.GetID();
                                                    while (db.SeKeyWords.Where(e => e.Id == skwid).ToList().Count > 0)
                                                    {
                                                        skwid = Public.GetID();
                                                    }
                                                    skw2.Id = skwid;
                                                    skw2.KeyWordId = countGW[i].Id;
                                                    skw2.SeId = itemS.Id;
                                                    skw2.CountNumber += "Chạy lần " + kc + ":" + countGW[i].Words + "/" + countGW[i].Count;
                                                    db.SeKeyWords.Add(skw2);
                                                    db.SaveChanges();
                                                }
                                                else
                                                {
                                                    var skw = db.SeKeyWords.Where(e => e.KeyWordId == skwi && e.SeId == itemS.Id).First();
                                                    skw.KeyWordId = countGW[i].Id;
                                                    skw.SeId = itemS.Id;
                                                    skw.CountNumber += "Chạy lần " + kc + ":" + countGW[i].Words + "/" + countGW[i].Count;
                                                    db.Entry(skw).State = EntityState.Modified;
                                                    db.SaveChanges();

                                                }
                                            }
                                        }
                                    }
                                }
                            }                                                     
                            catch { }
                            #endregion
                        }
                        #endregion

                        #region Tính x2
                        var gw = db.GroupWords.ToList();
                        int kkk = 1;
                        foreach (var itemGW in gw)
                        {
                            string gwid = itemGW.Id;
                            
                            List<ListVocabulary> x2 = new List<ListVocabulary>();
                            int dd = 0;

                            var S2 = db.SeKeyWords.Where(e => e.KeyWordId == gwid).ToList();
                            if (S2.Count > 0)
                            {
                                var V = db.Vocabularies.ToList();
                                #region Tính các giá trị
                                foreach (var itemV in V)
                                {
                                    int c1 = 0;

                                    //c1: là số lần xuất hiện của từ w trong câu thuộc về khía cạnh Ai.
                                    try
                                    {
                                        foreach (var itemA in S2)
                                        {
                                            string sW = "\\b" + Convert.ToString(itemV.Word) + "\\b";
                                            Regex thegex1 = new Regex(sW.Trim().ToLower());
                                            MatchCollection theMatches1 = thegex1.Matches(itemA.Sentens.ContentReview.Trim().ToLower());
                                            if (theMatches1.Count > 0)
                                            {
                                                c1 += theMatches1.Count;
                                            }
                                        }

                                        x2.Add(new ListVocabulary { Id = itemV.Id, C1 = c1, Words = itemV.Word });
                                    }
                                    catch { }

                                    //LogAddKeyWords kw = new LogAddKeyWords();
                                    //string laid = Public.GetID();
                                    //while (db.LogAddKeyWords.Where(e => e.Id == laid).ToList().Count > 0)
                                    //{
                                    //    laid = Public.GetID();
                                    //}
                                    //kw.Id = laid;
                                    //kw.GroupWord = gwid;
                                    //kw.Words = itemV.Word;
                                    //kw.Logs = "Lần chạy:" + kc + " có C1=" + c1;
                                    //db.LogAddKeyWords.Add(kw);
                                    //db.SaveChanges();

                                    dd++;
                                }
                                #endregion
                                #region Sắp xếp C1
                                try
                                {
                                    for (int ii = 0; ii < x2.Count - 1; ii++)
                                    {
                                        for (int j = ii + 1; j < x2.Count; j++)
                                        {
                                            if (x2[ii].C1 < x2[j].C1)
                                            {
                                                ListVocabulary tg = x2[ii];
                                                x2[ii] = x2[j];
                                                x2[j] = tg;
                                            }
                                        }
                                    }
                                }
                                catch { }
                                #endregion
                                #region Đưa các từ có tần xuất cao nhất vào khía cạnh Tj
                                try
                                {
                                    if (x2.Count > 0)
                                    {
                                        int N = int.Parse(x2[0].C1.ToString());
                                        foreach(var cc in x2)
                                        {
                                            if (cc.C1 > 0 && cc.C1==N)
                                            {
                                                //try
                                                //{
                                                string vw = cc.Words.Trim().ToLower();
                                                if (db.KeyWords.Where(e => e.Word.Trim().ToLower() == vw && e.GroupWordId == gwid).ToList().Count <= 0)
                                                {
                                                    KeyWord kw = new KeyWord();
                                                    string kwid = Public.GetID();
                                                    while (db.KeyWords.Where(e => e.Id == kwid).ToList().Count > 0)
                                                    {
                                                        kwid = Public.GetID();
                                                    }
                                                    kw.Id = kwid;
                                                    kw.Word = vw;
                                                    kw.GroupWordId = gwid;
                                                    kw.Type = 0;
                                                    kw.Logs = "Lần chạy:" + kc + " có C1=" + cc.C1;
                                                    db.KeyWords.Add(kw);
                                                    db.SaveChanges();
                                                }
                                                //}
                                                //catch { }
                                            }
                                        }
                                    }
                                }
                                catch { }

                                #endregion

                                #region Đưa các từ vào khía cạnh Ki
                                var v2 = db.Vocabularies.Where(e => e.TypeWord == "RB" || e.TypeWord == "JJ").ToList();

                                try
                                {
                                    foreach (var itemV2 in v2)
                                    {
                                        try
                                        {
                                            foreach (var itemA in S2)
                                            {
                                                string sW = "\\b" + Convert.ToString(itemV2.Word) + "\\b";
                                                Regex thegex1 = new Regex(sW.Trim().ToLower());
                                                MatchCollection theMatches1 = thegex1.Matches(itemA.Sentens.ContentReview.Trim().ToLower());
                                                if (theMatches1.Count > 0)
                                                {
                                                    string vw = itemV2.Word.Trim().ToLower();
                                                    if (db.KeyWords.Where(e => e.Word.Trim().ToLower() == vw && e.GroupWordId == gwid).ToList().Count <= 0)
                                                    {
                                                        KeyWord kw = new KeyWord();
                                                        string kwid = Public.GetID();
                                                        while (db.KeyWords.Where(e => e.Id == kwid).ToList().Count > 0)
                                                        {
                                                            kwid = Public.GetID();
                                                        }
                                                        kw.Id = kwid;
                                                        kw.Word = vw;
                                                        kw.GroupWordId = gwid;
                                                        kw.Type = kkk;
                                                        kw.Logs = "Lần chạy:" + kc ;
                                                        db.KeyWords.Add(kw);
                                                        db.SaveChanges();
                                                    }
                                                }
                                            }

                                        }
                                        catch { }
                                    }                                   
                                }
                                catch{ }
                                #endregion
                            }
                            
                            kkk++;

                        }
                        #endregion

                        //Kiểm tra cập nhật khía cạnh
                        //var w2 = db.KeyWords.Where(e => e.GroupWordId == k).ToList();
                        //if ((w2.Count - ww) >= 3)
                        //{
                        //    status = true;
                        //}
                        //else
                        //{
                        //    status = false;
                        //}

                    }
                    catch { }
                    kc++;
                }
            }
            return View();
        }

        //public ActionResult Details()
        //{
        //    var GC = db.GroupComents.ToList();
        //    var V = db.Vocabularies.ToList();
        //    foreach (var itemGC in GC)
        //    {
        //        //try
        //        //{
        //            var C = db.Comments.Where(e => e.GroupCommentId == itemGC.Id);
        //            foreach (var itemV in V)
        //            {
        //                if (db.VocabularyGroupComents.Where(e => e.GroupComentId == itemGC.Id && e.VocabularyId == itemV.Id).ToList().Count <= 0)
        //                {
        //                    foreach (var itemC in C)
        //                    {
        //                        //try
        //                        //{
        //                        string sS = "\\b" + Convert.ToString(itemV.Word) + "\\b";
        //                        Regex thegex2 = new Regex(sS.ToLower());
        //                        MatchCollection theMatches2 = thegex2.Matches(itemC.Comment1);
        //                        if (theMatches2.Count > 0)
        //                        {
        //                            VocabularyGroupComent vg = new VocabularyGroupComent();
        //                            string idvg = Public.GetID();
        //                            while (db.VocabularyGroupComents.Where(e => e.Id == idvg).ToList().Count > 0)
        //                            {
        //                                idvg = Public.GetID();
        //                            }
        //                            vg.Id = idvg;
        //                            vg.VocabularyId = itemV.Id;
        //                            vg.GroupComentId = itemGC.Id;
        //                            db.VocabularyGroupComents.Add(vg);

        //                            break;
        //                        }

        //                        //}
        //                        //catch { }
        //                    }
        //                    db.SaveChanges();
        //                }
        //                //}
        //                //catch { }
        //            }
        //        //}
        //        //catch { }

        //    }
        //    return View();
        //}


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