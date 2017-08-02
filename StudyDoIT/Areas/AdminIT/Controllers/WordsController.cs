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
        public double? Value { get; set; }
    }

    public class ListGroupKeyWord
    {
        public string Id { get; set; }
        public int? Count { get; set; }
    }

    public class ListCountKeyWord
    {
        public string Id { get; set; }
        public int? Count { get; set; }
    }

    public class WordsController : Controller
    {
        lCMSData db = new lCMSData();

        public ActionResult Index()
        {
            var GC = db.GroupComents.ToList();
            foreach (var itemGC in GC)
            {
                int kc = 0;
                while (kc < 5)
                {
                    try
                    {
                        #region Gán nhãn câu
                        var s = db.Sentenses.Where(e => e.Comment.GroupCommentId == itemGC.Id).ToList();
                        
                        //if (s.Count < 10)
                        //{
                        foreach (var itemS in s)
                        {
                            var gW = db.GroupWords.ToList();
                            List<ListCountKeyWord> countKW = new List<ListCountKeyWord>();
                            //List<ListGroupKeyWord> countW = new List<ListGroupKeyWord>();
                            List<ListGroupKeyWord> countGW = new List<ListGroupKeyWord>();
                            #region So khớp từ khóa
                            foreach (var itemgW in gW)
                            {
                                var w = db.KeyWords.Where(e => e.GroupWordId == itemgW.Id).ToList();
                                int www = 0;
                                foreach (var itemW in w)
                                {

                                    string sN = "\\b" + Convert.ToString(itemW.Word) + "\\b";
                                    Regex thegex = new Regex(sN.ToLower());
                                    MatchCollection theMatches = thegex.Matches(itemS.ContentReview);
                                    www += theMatches.Count;
                                    if (theMatches.Count > 0)
                                    {
                                        countKW.Add(new ListCountKeyWord { Count = theMatches.Count, Id = itemW.Id });
                                    }
                                }

                                countGW.Add(new ListGroupKeyWord { Id = itemgW.Id, Count = w.Count });
                            }
                            #endregion
                            #region Sắp xếp
                            if (countKW.Count > 0)
                            {
                                //Sắp xếp các countW
                                for (int i = 0; i < countKW.Count - 1; i++)
                                {
                                    for (int j = i + 1; j < countKW.Count; j++)
                                    {
                                        if (countKW[i].Count < countKW[j].Count)
                                        {
                                            ListCountKeyWord tg = countKW[i];
                                            countKW[i] = countKW[j];
                                            countKW[j] = tg;
                                        }
                                    }
                                }
                            }
                            #endregion
                            #region Gán nhãn từ khóa
                            if (countKW.Count > 0)
                            {
                                if (countKW[0].Count > 0)
                                {
                                    string kwid = countKW[0].Id;
                                    try
                                    {
                                        if (db.SeKeyWords.Where(e => e.KeyWordId == kwid && e.SeId == itemS.Id).ToList().Count <= 0)
                                        {
                                            SeKeyWord skw = new SeKeyWord();
                                            skw.Id = Public.GetID();
                                            skw.KeyWordId = countKW[0].Id;
                                            skw.SeId = itemS.Id;
                                            db.SeKeyWords.Add(skw);
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            var skw = db.SeKeyWords.Where(e => e.KeyWordId == kwid).First();                             
                                            skw.KeyWordId = countKW[0].Id;
                                            skw.SeId = itemS.Id;
                                            db.Entry(skw).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                    }
                                    catch { }

                                    try
                                    {
                                        
                                        for (int i = 1; i < 5; i++)
                                        {
                                            if (countKW.Count > i)
                                            {
                                                if (countKW[i].Count == countKW[0].Count)
                                                {
                                                    try
                                                    {
                                                        string skwi=countKW[i].Id;
                                                        if (db.SeKeyWords.Where(e => e.KeyWordId == skwi && e.SeId == itemS.Id).ToList().Count <= 0)
                                                        {
                                                            SeKeyWord skw2 = new SeKeyWord();
                                                            skw2.Id = Public.GetID();
                                                            skw2.KeyWordId = countKW[i].Id;
                                                            skw2.SeId = itemS.Id;
                                                            db.SeKeyWords.Add(skw2);
                                                            db.SaveChanges();
                                                        }
                                                        else
                                                        {
                                                            var skw = db.SeKeyWords.Find(kwid);
                                                            if (skwi != countKW[0].Id)
                                                            {
                                                                skw.KeyWordId = countKW[i].Id;
                                                                skw.SeId = itemS.Id;
                                                                db.Entry(skw).State = EntityState.Modified;
                                                                db.SaveChanges();
                                                            }
                                                        }

                                                    }
                                                    catch { }
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    catch { }
                                }
                            }
                            #endregion
                        }
                        #endregion
                        #region Tính x2
                        var gw = db.GroupWords.ToList();
                        int kkk = 1;
                        foreach (var itemGW in gw)
                        {
                            string k = itemGW.Id;
                            int c1 = 0, c2 = 0, c3 = 0, c4 = 0, c = 0;
                            int s1 = 0, s2 = 0, s3 = 0;
                            List<ListVocabulary> x2 = new List<ListVocabulary>();
                            int dd = 0;

                            var A = db.SeKeyWords.Where(e => e.KeyWord.GroupWordId == k).ToList();
                            if (A.Count > 0)
                            {
                                var VG = db.VocabularyGroupComents.Where(e => e.GroupComentId == itemGC.Id).ToList();
                                #region Tính các giá trị
                                foreach (var itemV in VG)
                                {
                                    foreach (var itemA in A)
                                    {
                                        string sW = "\\b" + Convert.ToString(itemV.Vocabulary.Word) + "\\b";
                                        Regex thegex1 = new Regex(sW.ToLower());
                                        MatchCollection theMatches1 = thegex1.Matches(itemA.Sentens.ContentReview);
                                        if (theMatches1.Count > 0)
                                        {
                                            c1 += theMatches1.Count;
                                            s1++;
                                        }
                                    }
                                    c3 = A.Count - s1;

                                    foreach (var itemSs in s)
                                    {
                                        string sS = "\\b" + Convert.ToString(itemV.Vocabulary.Word) + "\\b";
                                        Regex thegex2 = new Regex(sS.ToLower());
                                        MatchCollection theMatches2 = thegex2.Matches(itemSs.ContentReview);
                                        if (theMatches2.Count > 0)
                                        {
                                            c += theMatches2.Count;
                                            s2++;
                                        }
                                    }
                                    c2 = c - c1;

                                    s3 = s.Count - s2;
                                    c4 = s3 - c3;
                                    double xxx = c * Math.Pow((c1 * c4 - c2 * c3), 2);
                                    double xxxx = (double)(c1 + c3) * (c2 + c4) * (c1 + c2) * (c3 + c4);
                                    double xx = 0;
                                    if (xxxx != 0.0)
                                    {
                                        xx = (double)(xxx / xxxx);
                                    }
                                    x2.Add(new ListVocabulary { Id = itemV.VocabularyId, Value = xx });
                                    dd++;
                                }
                                #endregion
                                #region Sắp xếp x2
                                for (int ii = 0; ii < x2.Count - 1; ii++)
                                {
                                    for (int j = ii + 1; j < x2.Count; j++)
                                    {
                                        if (x2[ii].Value < x2[j].Value)
                                        {
                                            ListVocabulary tg = x2[ii];
                                            x2[ii] = x2[j];
                                            x2[j] = tg;
                                        }
                                    }
                                }
                                #endregion
                                #region Đưa 3, 4, 5 từ đầu tiên vào khía cạnh Tj
                                if (x2.Count > 0)
                                {
                                    for (int i = 0; i < 5; i++)
                                    {
                                        if (x2[i].Value > 0.0)
                                        {
                                            try
                                            {
                                                string vw = db.Vocabularies.Find(x2[i].Id).Word;
                                                if (db.KeyWords.Where(e => e.Word == vw && e.GroupWordId == k).ToList().Count <= 0)
                                                {
                                                    KeyWord kw = new KeyWord();
                                                    kw.Id = Public.GetID();
                                                    kw.Word = vw;
                                                    kw.GroupWordId = k;
                                                    kw.Type = kkk;
                                                    db.KeyWords.Add(kw);
                                                    db.SaveChanges();
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                }
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

        //
        // GET: /AdminIT/Words/Details/5
        public ActionResult Details()
        {
            var GC = db.GroupComents.ToList();
            var V = db.Vocabularies.ToList();
            foreach (var itemGC in GC)
            {
                //try
                //{
                    var C = db.Comments.Where(e => e.GroupCommentId == itemGC.Id);
                    foreach (var itemV in V)
                    {
                        //try
                        //{
                            foreach (var itemC in C)
                            {
                                //try
                                //{
                                string sS = "\\b" + Convert.ToString(itemV.Word) + "\\b";
                                    Regex thegex2 = new Regex(sS.ToLower());
                                    MatchCollection theMatches2 = thegex2.Matches(itemC.Comment1);
                                    if (theMatches2.Count > 0)
                                    {
                                        VocabularyGroupComent data = new VocabularyGroupComent();
                                        string idvg = Public.GetID();
                                        while (db.VocabularyGroupComents.Where(e => e.Id == idvg).ToList().Count > 0)
                                        {
                                            idvg = Public.GetID();
                                        }
                                        data.Id = idvg;
                                        data.VocabularyId = itemV.Id;
                                        data.GroupComentId = itemGC.Id;
                                        db.VocabularyGroupComents.Add(data);
                                        db.SaveChanges();
                                        break;
                                    }
                                //}
                                //catch { }
                            }
                        //}
                        //catch { }
                    }
                //}
                //catch { }

            }
            return View();
        }

    }
}
