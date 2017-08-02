using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using StudyDoIT.Models.NLP;
using StudyDoIT.Models.Common;
using System.Text.RegularExpressions;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using Console = System.Console;
using java.util;
using java.io;

namespace StudyDoIT.Areas.AdminIT.Controllers
{
    public class TranningController : Controller
    {
        lCMSData db = new lCMSData();

        public class ListKeyWordsCount
        {
            public string KeywordId { get; set; }
            public string GroupKeywordId { get; set; }
            public int? C1 { get; set; }
            public int? C2 { get; set; }
            public int? C3 { get; set; }
            public int? C4 { get; set; }
            public int? C5 { get; set; }
            public int? Total { get; set; }
        }

        public class ListKeyWordScore
        {
            public string Id { get; set; }
            public decimal? Score { get; set; }
        }

        public class ListComment
        {
            public string Id { get; set; }
            public string Comment { get; set; }
            public decimal? Rating { get; set; }
        }

        #region
        //public ActionResult Index()
        //{
        //    #region Làm mới bảng thông kê
        //    var kcc = db.KeywordsCounts.ToList();
        //    foreach (var itemKCC in kcc)
        //    {
        //        db.KeywordsCounts.Where(e => e.GroupKeyWordId == itemKCC.GroupKeyWordId).FirstOrDefault();
        //        db.KeywordsCounts.Remove(itemKCC);
        //        db.SaveChanges();
        //    }

        //    var listKWW = db.KeyWords.ToList();
        //    foreach (var itemKW in listKWW)
        //    {
        //        var ikw = db.KeyWords.Find(itemKW.Id);
        //        ikw.C1 = 0;
        //        ikw.C2 = 0;
        //        ikw.C3 = 0;
        //        ikw.C4 = 0;
        //        ikw.C5 = 0;
        //        ikw.P1 = 0;
        //        ikw.P2 = 0;
        //        ikw.P3 = 0;
        //        ikw.P4 = 0;
        //        ikw.P5 = 0;
        //        ikw.Total = 0;

        //        db.Entry(ikw).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }

        //    var listGWW = db.GroupWords.ToList();
        //    foreach (var itemGW in listGWW)
        //    {
        //        var igw = db.GroupWords.Find(itemGW.Id);
        //        igw.C1 = 0;
        //        igw.C2 = 0;
        //        igw.C3 = 0;
        //        igw.C4 = 0;
        //        igw.C5 = 0;
        //        igw.Total = 0;

        //        db.Entry(igw).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    #endregion

        //    #region Thống kê
        //    var listGW = db.GroupWords.ToList();
        //    foreach (var itemGW in listGW)
        //    {
        //        #region Thông kê trên từng tập review

        //        var listC = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id).ToList();

        //        if (listC.Count > 0)
        //        {
        //            var kw = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id).ToList();
        //            foreach (var itemW in kw)
        //            {
        //                foreach (var itemC in listC)
        //                {
        //                    string sN = "\\b" + Convert.ToString(itemW.Word) + "\\b";
        //                    Regex thegex = new Regex(sN.ToLower());
        //                    MatchCollection theMatches = thegex.Matches(itemC.Sentens.ContentReview.Trim().ToLower());
        //                    if (theMatches.Count > 0)
        //                    {
        //                        decimal rv = decimal.Parse(itemC.Sentens.Comment.Rating.ToString());

        //                        var gw = db.GroupWords.Find(itemGW.Id);
        //                        gw.Total += 1;
        //                        if (rv == 5)
        //                        {
        //                            gw.C5 += 1;
        //                        }
        //                        else if (rv == 4)
        //                        {
        //                            gw.C4 += 1;
        //                        }
        //                        else if (rv == 3)
        //                        {
        //                            gw.C3 += 1;
        //                        }
        //                        else if (rv == 2)
        //                        {
        //                            gw.C2 += 1;
        //                        }
        //                        else if (rv == 1)
        //                        {
        //                            gw.C1 += 1;
        //                        }

        //                        var itemKC = db.KeywordsCounts.Where(e => e.GroupKeyWordId == itemGW.Id && e.KeyWord == itemW.Id).ToList();
        //                        if (itemKC.Count <= 0)
        //                        {                                  
        //                            KeywordsCount kc = new KeywordsCount();
        //                            kc.GroupKeyWordId = itemGW.Id;
        //                            kc.KeyWord = itemW.Id;
        //                            kc.Total = theMatches.Count;
                                    
        //                            if (rv == 5)
        //                            {
        //                                kc.C1 = 0;
        //                                kc.C2 = 0;
        //                                kc.C3 = 0;
        //                                kc.C4 = 0;
        //                                kc.C5 = theMatches.Count;
        //                                kc.P1 = 0;
        //                                kc.P2 = 0;
        //                                kc.P3 = 0;
        //                                kc.P4 = 0;
        //                                kc.P5 = 1;
        //                            }
        //                            else if (rv == 4)
        //                            {
        //                                kc.C1 = 0;
        //                                kc.C2 = 0;
        //                                kc.C3 = 0;
        //                                kc.C4 = theMatches.Count;
        //                                kc.C5 = 0;
        //                                kc.P1 = 0;
        //                                kc.P2 = 0;
        //                                kc.P3 = 0;
        //                                kc.P4 = 1;
        //                                kc.P5 = 0;
        //                            }
        //                            else if (rv == 3)
        //                            {
        //                                kc.C1 = 0;
        //                                kc.C2 = 0;
        //                                kc.C3 = theMatches.Count;
        //                                kc.C4 = 0;
        //                                kc.C5 = 0;
        //                                kc.P1 = 0;
        //                                kc.P2 = 0;
        //                                kc.P3 = 1;
        //                                kc.P4 = 0;
        //                                kc.P5 = 0;
        //                            }
        //                            else if (rv == 2)
        //                            {
        //                                kc.C1 = 0;
        //                                kc.C2 = theMatches.Count;
        //                                kc.C3 = 0;
        //                                kc.C4 = 0;
        //                                kc.C5 = 0;
        //                                kc.P1 = 0;
        //                                kc.P2 = 1;
        //                                kc.P3 = 0;
        //                                kc.P4 = 0;
        //                                kc.P5 = 0;
        //                            }
        //                            else if (rv == 1)
        //                            {
        //                                kc.C1 = theMatches.Count;
        //                                kc.C2 = 0;
        //                                kc.C3 = 0;
        //                                kc.C4 = 0;
        //                                kc.C5 = 0;
        //                                kc.P1 = 1;
        //                                kc.P2 = 0;
        //                                kc.P3 = 0;
        //                                kc.P4 = 0;
        //                                kc.P5 = 0;
        //                            }

        //                            db.Entry(gw).State = EntityState.Modified;
        //                            db.KeywordsCounts.Add(kc);
        //                            db.SaveChanges();
        //                        }
        //                        else
        //                        {
        //                            var kc = itemKC.FirstOrDefault();
        //                            kc.Total += theMatches.Count;
        //                            if (rv == 5)
        //                            {
        //                                kc.C5 += theMatches.Count;
        //                                kc.P5 = Math.Round(((decimal)kc.C5 / (decimal)kc.Total), 2);
        //                                kc.P4 = Math.Round(((decimal)kc.C4 / (decimal)kc.Total), 2);
        //                                kc.P3 = Math.Round(((decimal)kc.C3 / (decimal)kc.Total), 2);
        //                                kc.P2 = Math.Round(((decimal)kc.C2 / (decimal)kc.Total), 2);
        //                                kc.P1 = Math.Round(((decimal)kc.C1 / (decimal)kc.Total), 2);
        //                            }
        //                            else if (rv == 4)
        //                            {
        //                                kc.C4 += theMatches.Count;
        //                                kc.P5 = Math.Round(((decimal)kc.C5 / (decimal)kc.Total), 2);
        //                                kc.P4 = Math.Round(((decimal)kc.C4 / (decimal)kc.Total), 2);
        //                                kc.P3 = Math.Round(((decimal)kc.C3 / (decimal)kc.Total), 2);
        //                                kc.P2 = Math.Round(((decimal)kc.C2 / (decimal)kc.Total), 2);
        //                                kc.P1 = Math.Round(((decimal)kc.C1 / (decimal)kc.Total), 2);
        //                            }
        //                            else if (rv == 3)
        //                            {
        //                                kc.C3 += theMatches.Count;
        //                                kc.P5 = Math.Round(((decimal)kc.C5 / (decimal)kc.Total), 2);
        //                                kc.P4 = Math.Round(((decimal)kc.C4 / (decimal)kc.Total), 2);
        //                                kc.P3 = Math.Round(((decimal)kc.C3 / (decimal)kc.Total), 2);
        //                                kc.P2 = Math.Round(((decimal)kc.C2 / (decimal)kc.Total), 2);
        //                                kc.P1 = Math.Round(((decimal)kc.C1 / (decimal)kc.Total), 2);
        //                            }
        //                            else if (rv == 2)
        //                            {
        //                                kc.C2 += theMatches.Count;
        //                                kc.P5 = Math.Round(((decimal)kc.C5 / (decimal)kc.Total), 2);
        //                                kc.P4 = Math.Round(((decimal)kc.C4 / (decimal)kc.Total), 2);
        //                                kc.P3 = Math.Round(((decimal)kc.C3 / (decimal)kc.Total), 2);
        //                                kc.P2 = Math.Round(((decimal)kc.C2 / (decimal)kc.Total), 2);
        //                                kc.P1 = Math.Round(((decimal)kc.C1 / (decimal)kc.Total), 2);
        //                            }
        //                            else if (rv == 1)
        //                            {
        //                                kc.C1 += theMatches.Count;
        //                                kc.P5 = Math.Round(((decimal)kc.C5 / (decimal)kc.Total), 2);
        //                                kc.P4 = Math.Round(((decimal)kc.C4 / (decimal)kc.Total), 2);
        //                                kc.P3 = Math.Round(((decimal)kc.C3 / (decimal)kc.Total), 2);
        //                                kc.P2 = Math.Round(((decimal)kc.C2 / (decimal)kc.Total), 2);
        //                                kc.P1 = Math.Round(((decimal)kc.C1 / (decimal)kc.Total), 2);
        //                            }

        //                            db.Entry(kc).State = EntityState.Modified;
        //                            db.SaveChanges();
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //        #endregion

        //        #region Thông kê điểm toàn tập review

        //        var listKW = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id).ToList();

        //        var listCM = db.Comments.ToList();
        //        foreach (var itemKW in listKW)
        //        {
        //            foreach (var itemCM in listCM)
        //            {
        //                string sN = "\\b" + Convert.ToString(itemKW.Word) + "\\b";
        //                Regex thegex = new Regex(sN.ToLower());
        //                MatchCollection theMatches = thegex.Matches(itemCM.Comment1.ToLower());
        //                if (theMatches.Count > 0)
        //                {
        //                    var itemK = db.KeyWords.Where(e => e.Id == itemKW.Id).ToList();

        //                    var kc = itemK.FirstOrDefault();
        //                    kc.Total += theMatches.Count;
        //                    decimal rv = decimal.Parse(itemCM.Rating.ToString());
        //                    if (rv == 5)
        //                    {
        //                        kc.C5 += theMatches.Count;
        //                        kc.P5 = (decimal)kc.C5 / (decimal)kc.Total;
        //                        kc.P4 = (decimal)kc.C4 / (decimal)kc.Total;
        //                        kc.P3 = (decimal)kc.C3 / (decimal)kc.Total;
        //                        kc.P2 = (decimal)kc.C2 / (decimal)kc.Total;
        //                        kc.P1 = (decimal)kc.C1 / (decimal)kc.Total;
        //                    }
        //                    else if (rv == 4)
        //                    {
        //                        kc.C4 += theMatches.Count;
        //                        kc.P5 = (decimal)kc.C5 / (decimal)kc.Total;
        //                        kc.P4 = (decimal)kc.C4 / (decimal)kc.Total;
        //                        kc.P3 = (decimal)kc.C3 / (decimal)kc.Total;
        //                        kc.P2 = (decimal)kc.C2 / (decimal)kc.Total;
        //                        kc.P1 = (decimal)kc.C1 / (decimal)kc.Total;
        //                    }
        //                    else if (rv == 3)
        //                    {
        //                        kc.C3 += theMatches.Count;
        //                        kc.P5 = (decimal)kc.C5 / (decimal)kc.Total;
        //                        kc.P4 = (decimal)kc.C4 / (decimal)kc.Total;
        //                        kc.P3 = (decimal)kc.C3 / (decimal)kc.Total;
        //                        kc.P2 = (decimal)kc.C2 / (decimal)kc.Total;
        //                        kc.P1 = (decimal)kc.C1 / (decimal)kc.Total;
        //                    }
        //                    else if (rv == 2)
        //                    {
        //                        kc.C2 += theMatches.Count;
        //                        kc.P5 = (decimal)kc.C5 / (decimal)kc.Total;
        //                        kc.P4 = (decimal)kc.C4 / (decimal)kc.Total;
        //                        kc.P3 = (decimal)kc.C3 / (decimal)kc.Total;
        //                        kc.P2 = (decimal)kc.C2 / (decimal)kc.Total;
        //                        kc.P1 = (decimal)kc.C1 / (decimal)kc.Total;
        //                    }
        //                    else if (rv == 1)
        //                    {
        //                        kc.C1 += theMatches.Count;
        //                        kc.P5 = (decimal)kc.C5 / (decimal)kc.Total;
        //                        kc.P4 = (decimal)kc.C4 / (decimal)kc.Total;
        //                        kc.P3 = (decimal)kc.C3 / (decimal)kc.Total;
        //                        kc.P2 = (decimal)kc.C2 / (decimal)kc.Total;
        //                        kc.P1 = (decimal)kc.C1 / (decimal)kc.Total;
        //                    }

        //                    db.Entry(kc).State = EntityState.Modified;
        //                    db.SaveChanges();
        //                }
        //            }
        //        }
        //        #endregion
        //    }
        //    #endregion

        //    return View();
        //}
        #endregion

        public ActionResult Index()
        {
            IEnumerable<GroupComent> data = (IEnumerable<GroupComent>)db.GroupComents.ToList();
            ViewBag.GroupComent = new SelectList(data, "Id", "Name");

            return View();
        }

        //Thông kê đếm số câu và công thức làm trơn
        public ActionResult Tranning(string idgc)
        {
            string idp = "170329112813869";
            //idgc = "170601082051527";
            #region Gán nhãn câu
            //ClearAddLebels(idgc);
            //AddLabels(idgc, idp);
            #endregion

            #region Làm mới bảng thông kê
            //var kcc = db.KeywordsCounts.Where(e => e.GroupCommentId == idgc).ToList();
            //foreach (var itemKCC in kcc)
            //{
            //    db.KeywordsCounts.Where(e => e.GroupKeyWordId == itemKCC.GroupKeyWordId).FirstOrDefault();
            //    db.KeywordsCounts.Remove(itemKCC);
            //    db.SaveChanges();
            //}

            var listKWW = db.KeyWords.Where(e => e.GroupWord.ProductId == idp && e.TypeWord == 1 && e.Type != 0).ToList();
            //var listKWW = from kw in db.KeyWords where ((kw.GroupWordId == idgc && kw.TypeWord==1) || kw.Type==0) select new { Id = kw.Id, Word = kw.Word };
            foreach (var itemKW in listKWW)
            {
                db.KeyWords.Remove(itemKW);
                db.SaveChanges();
            }
            
            var listGWW = db.GroupWords.Where(e => e.ProductId == idp).ToList();
            foreach (var itemGW in listGWW)
            {
                var igw = db.GroupWords.Find(itemGW.Id);
                igw.C1 = 0;
                igw.C2 = 0;
                igw.C3 = 0;
                igw.C4 = 0;
                igw.C5 = 0;
                igw.Total = 0;

                db.Entry(igw).State = EntityState.Modified;
                db.SaveChanges();
            }

            //var vc = db.Vocabularies.Where(e => e.GroupCommentId == idgc && e.Type!=1).ToList();
            //foreach (var itemVC in vc)
            //{
            //    var v = db.Vocabularies.Find(itemVC.Id);
            //    db.Vocabularies.Remove(v);
            //    db.SaveChanges();
            //}
            #endregion

            #region Cập nhật từ điển
            UpdateVocabulary(listGWW, idgc);
            #endregion

            #region Thống kê
            Statistics2(idp, idgc);
            
            #endregion

            return View();
        }


        private void UpdateVocabulary(IEnumerable<GroupWord> listGWW, string idgc)
        {
            string urlRoot = System.IO.Path.Combine(Server.MapPath("~/Uploads/english-left3words"), "english-left3words-distsim.tagger");
            var tagger = new edu.stanford.nlp.tagger.maxent.MaxentTagger(urlRoot);

            int k = 1;
            foreach (var itemGW in listGWW)
            {
                k++;
                var data = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id).ToList();
                foreach (var item in data)
                {
                    //Xác định điểm
                    var score = db.GroupWordComments.Where(e => e.CommentId == item.Sentens.CommentId && e.GroupWordId == itemGW.Id).Select(e => e.Score).FirstOrDefault();
                    if (score != null)
                    {
                        int rv = (int)score;

                        itemGW.Total += 1;

                        if (rv == 5)
                        {
                            itemGW.C5 += 1;
                        }
                        else if (rv == 4)
                        {
                            itemGW.C4 += 1;
                        }
                        else if (rv == 3)
                        {
                            itemGW.C3 += 1;
                        }
                        else if (rv == 2)
                        {
                            itemGW.C2 += 1;
                        }
                        else if (rv == 1)
                        {
                            itemGW.C1 += 1;
                        }
                        else
                        {
                            LogAddKeyWord akw = new LogAddKeyWord();
                            akw.Id = Public.GetID2();
                            akw.GroupWordId = itemGW.Id;
                            akw.Logs=score+"";
                            db.LogAddKeyWords.Add(akw);
                            db.SaveChanges();
                        }
                    }
                    //Xử lý từ
                    var text = item.Sentens.ContentReview;
                    var sentences = edu.stanford.nlp.tagger.maxent.MaxentTagger.tokenizeText(new StringReader(text)).toArray();
                    foreach (ArrayList sentence in sentences)
                    {
                        var taggedSentence = tagger.tagSentence(sentence);
                        string[] str1 = taggedSentence.ToString().Split(',', '[', ']');

                        #region Tách có tạo từ ghép
                        //Kiểm tra từ ghép
                        for (int i = 0; i < str1.Length; i++)
                        {
                            string[] str2 = str1[i].ToString().Split('/');

                            #region Tạo từ đơn
                            if (str2.Length > 1)
                            {
                                if (str2[1].Trim() == "JJ" || str2[1].Trim() == "JJR" || str2[1].Trim() == "JJS"
                                                    || str2[1].Trim() == "RB" || str2[1].Trim() == "RBR" || str2[1].Trim() == "RBS")
                                {
                                    if (str2[0].Trim().Count() > 1)
                                    {
                                        string word = str2[0].Trim().ToLower();
                                        var datav = db.KeyWords.Where(e => e.Word.Trim().ToLower().Equals(word) && e.GroupCommentId == idgc && e.GroupWordId == itemGW.Id && e.TypeWord==1).ToList();
                                        if (datav.Count <= 0)
                                        {
                                            KeyWord data4 = new KeyWord();
                                            if (score != null)
                                            {
                                                int rv = (int)score;
                                                if (rv == 5)
                                                {
                                                    data4.C1 = 0;
                                                    data4.C2 = 0;
                                                    data4.C3 = 0;
                                                    data4.C4 = 0;
                                                    data4.C5 = 1;
                                                }
                                                else if (rv == 4)
                                                {
                                                    data4.C1 = 0;
                                                    data4.C2 = 0;
                                                    data4.C3 = 0;
                                                    data4.C4 = 1;
                                                    data4.C5 = 0;
                                                }
                                                else if (rv == 3)
                                                {
                                                    data4.C1 = 0;
                                                    data4.C2 = 0;
                                                    data4.C3 = 1;
                                                    data4.C4 = 0;
                                                    data4.C5 = 0;
                                                }
                                                else if (rv == 2)
                                                {
                                                    data4.C1 = 0;
                                                    data4.C2 = 1;
                                                    data4.C3 = 0;
                                                    data4.C4 = 0;
                                                    data4.C5 = 0;
                                                }
                                                else if (rv == 1)
                                                {
                                                    data4.C1 = 1;
                                                    data4.C2 = 0;
                                                    data4.C3 = 0;
                                                    data4.C4 = 0;
                                                    data4.C5 = 0;
                                                }
                                                else
                                                {
                                                    LogAddKeyWord akw = new LogAddKeyWord();
                                                    akw.Id = Public.GetID2();
                                                    akw.GroupWordId = itemGW.Id;
                                                    akw.Logs = score + "";
                                                    db.LogAddKeyWords.Add(akw);
                                                    db.SaveChanges();
                                                }
                                                data4.Total = 1;
                                            }
                                            else
                                            {
                                                data4.C1 = 0;
                                                data4.C2 = 0;
                                                data4.C3 = 0;
                                                data4.C4 = 0;
                                                data4.C5 = 0;
                                                data4.Total = 0;
                                            }
                                           
                                            string idkw = Public.GetID();
                                            while (db.KeyWords.Where(e => e.Id == idkw).Count() > 0)
                                            {
                                                idkw = Public.GetID();
                                            }
                                            data4.Id = idkw;
                                            data4.Word = word;
                                            data4.GroupWordId = itemGW.Id;
                                            data4.GroupCommentId = idgc;
                                            data4.TypeWord = 1;
                                            data4.Logs = "Thêm tính từ hoặc trạng từ";
                                            data4.Type = k-1;                                           
                                            data4.P1 = 0;
                                            data4.P2 = 0;
                                            data4.P3 = 0;
                                            data4.P4 = 0;
                                            data4.P5 = 0;                                            
                                            db.KeyWords.Add(data4);
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            var data6 = datav.FirstOrDefault();

                                            if (score != null)
                                            {
                                                int rv = (int)score;

                                                if (rv == 5)
                                                {
                                                    data6.C5 += 1;
                                                }
                                                else if (rv == 4)
                                                {
                                                    data6.C4 += 1;
                                                }
                                                else if (rv == 3)
                                                {
                                                    data6.C3 += 1;
                                                }
                                                else if (rv == 2)
                                                {
                                                    data6.C2 += 1;
                                                }
                                                else if (rv == 1)
                                                {
                                                    data6.C1 += 1;
                                                }
                                                else
                                                {
                                                    LogAddKeyWord akw = new LogAddKeyWord();
                                                    akw.Id = Public.GetID2();
                                                    akw.GroupWordId = itemGW.Id;
                                                    akw.Logs = score + "";
                                                    db.LogAddKeyWords.Add(akw);
                                                    db.SaveChanges();
                                                }

                                                data6.Total += 1;
                                            }

                                            db.Entry(data6).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }
                            #endregion

                            string[] str3 = null;
                            if (i < str1.Length - 1)
                            {
                                str3 = str1[i + 1].ToString().Split('/');
                            }
                            string str4 = "";
                            string type = "JJ";
                            if (str3 != null)
                            {
                                if (str2.Length > 1 && str3.Length > 1)
                                {
                                    #region Tạo từ ghép
                                    if (str2[1].Trim() == "JJ" && (str3[1].Trim() == "NN" || str3[1].Trim() == "NNS")) //Luật 1
                                    {
                                        str4 = str2[0].Trim() + " " + str3[0].Trim();
                                    }
                                    else if ((str2[1].Trim() == "RB" || str2[1].Trim() == "RBR" || str2[1].Trim() == "RBS") && str3[1].Trim() == "JJ") //Luật 2
                                    {
                                        str4 = str2[0].Trim() + " " + str3[0].Trim();
                                    }
                                    else if (str2[1].Trim() == "JJ" && str3[1].Trim() == "JJ") //Luật 3
                                    {
                                        str4 = str2[0].Trim() + " " + str3[0].Trim();
                                    }
                                    else if ((str2[1].Trim() == "NN" || str2[1].Trim() == "NNS") && str3[1].Trim() == "JJ") //Luật 4
                                    {
                                        str4 = str2[0].Trim() + " " + str3[0].Trim();
                                    }
                                    else if ((str2[1].Trim() == "RB" || str2[1].Trim() == "RBR" || str2[1].Trim() == "RBS")
                                        && (str3[1].Trim() == "VB" || str3[1].Trim() == "VBD" || str3[1].Trim() == "VBN" || str3[1].Trim() == "VBG")) //Luật 5
                                    {
                                        str4 = str2[0].Trim() + " " + str3[0].Trim();
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                str4 = str2[0].Trim();
                                if (str2.Length > 1)
                                {
                                    type = str2[1].Trim();
                                }
                            }

                            //cập nhật từ điển
                            if (str4.Trim().Count() > 1)
                            {
                                string word = str4.Trim().ToLower();
                                var data2 = db.KeyWords.Where(e => e.Word.Trim().ToLower().Equals(word) && e.GroupCommentId == idgc && e.GroupWordId == itemGW.Id && e.TypeWord == 1).ToList();
                                if (data2.Count <= 0)
                                {
                                    KeyWord data4 = new KeyWord();
                                    if (score != null)
                                    {
                                        int rv = (int)Math.Round(score, 0);
                                        if (rv == 5)
                                        {
                                            data4.C1 = 0;
                                            data4.C2 = 0;
                                            data4.C3 = 0;
                                            data4.C4 = 0;
                                            data4.C5 = 1;
                                        }
                                        else if (rv == 4)
                                        {
                                            data4.C1 = 0;
                                            data4.C2 = 0;
                                            data4.C3 = 0;
                                            data4.C4 = 1;
                                            data4.C5 = 0;
                                        }
                                        else if (rv == 3)
                                        {
                                            data4.C1 = 0;
                                            data4.C2 = 0;
                                            data4.C3 = 1;
                                            data4.C4 = 0;
                                            data4.C5 = 0;
                                        }
                                        else if (rv == 2)
                                        {
                                            data4.C1 = 0;
                                            data4.C2 = 1;
                                            data4.C3 = 0;
                                            data4.C4 = 0;
                                            data4.C5 = 0;
                                        }
                                        else if (rv == 1)
                                        {
                                            data4.C1 = 1;
                                            data4.C2 = 0;
                                            data4.C3 = 0;
                                            data4.C4 = 0;
                                            data4.C5 = 0;
                                        }
                                        else
                                        {
                                            LogAddKeyWord akw = new LogAddKeyWord();
                                            akw.Id = Public.GetID2();
                                            akw.GroupWordId = itemGW.Id;
                                            akw.Logs = score + "";
                                            db.LogAddKeyWords.Add(akw);
                                            db.SaveChanges();
                                        }
                                        data4.Total = 1;
                                    }
                                    else
                                    {
                                        data4.C1 = 0;
                                        data4.C2 = 0;
                                        data4.C3 = 0;
                                        data4.C4 = 0;
                                        data4.C5 = 0;
                                        data4.Total = 0;
                                    }

                                    string idkw = Public.GetID();
                                    while (db.KeyWords.Where(e => e.Id == idkw).Count() > 0)
                                    {
                                        idkw = Public.GetID();
                                    }
                                    data4.Id = idkw;
                                    data4.Word = word;
                                    data4.GroupWordId = itemGW.Id;
                                    data4.GroupCommentId = idgc;
                                    data4.TypeWord = 1;
                                    data4.Logs = "Thêm tính từ hoặc trạng từ";
                                    data4.Type = k - 1;
                                    data4.P1 = 0;
                                    data4.P2 = 0;
                                    data4.P3 = 0;
                                    data4.P4 = 0;
                                    data4.P5 = 0;
                                    db.KeyWords.Add(data4);

                                    db.SaveChanges();
                                }
                                else
                                {
                                    var data6 = data2.FirstOrDefault();
                                    if (score != null)
                                    {
                                        int rv = (int)Math.Round(score, 0);

                                        if (rv == 5)
                                        {
                                            data6.C5 += 1;
                                        }
                                        else if (rv == 4)
                                        {
                                            data6.C4 += 1;
                                        }
                                        else if (rv == 3)
                                        {
                                            data6.C3 += 1;
                                        }
                                        else if (rv == 2)
                                        {
                                            data6.C2 += 1;
                                        }
                                        else if (rv == 1)
                                        {
                                            data6.C1 += 1;
                                        }
                                        else
                                        {
                                            LogAddKeyWord akw = new LogAddKeyWord();
                                            akw.Id = Public.GetID2();
                                            akw.GroupWordId = itemGW.Id;
                                            akw.Logs = score + "";
                                            db.LogAddKeyWords.Add(akw);
                                            db.SaveChanges();
                                        }

                                        data6.Total += 1;
                                    }
                                    db.Entry(data6).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                        #endregion
                    }

                }
                db.Entry(itemGW).State = EntityState.Modified;
                db.SaveChanges();
            }
        }       

        private void UpdateKeyWord(IEnumerable<GroupWord> gw, string idgc, int kc)
        {
            #region Cập nhật Ck và TWk
            int kkk = 1;
            foreach (var itemGW in gw)
            {
                string gwid = itemGW.Id;

                List<ListVocabulary> x2 = new List<ListVocabulary>();
                int dd = 0;

                var S2 = db.SeKeyWords.Where(e => e.KeyWordId == gwid && e.Sentens.Comment.GroupCommentId == idgc).ToList();
                if (S2.Count > 0)
                {
                    var V = db.Vocabularies.Where(e => e.GroupCommentId == idgc && (e.TypeWord == "NN" || e.TypeWord == "NNP"
                        || e.TypeWord == "NNS" || e.TypeWord == "NNPS")).ToList();
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
                    #region Đưa các từ có tần xuất cao nhất vào khía cạnh Ck, các từ còn lại lớn hơn 0 vào TWk
                    try
                    {
                        if (x2.Count > 0)
                        {
                            int N = int.Parse(x2[0].C1.ToString());
                            foreach (var cc in x2)
                            {
                                if (cc.C1 > 0 && cc.C1 == N)
                                {
                                    //try
                                    //{
                                    string vw = cc.Words.Trim().ToLower();
                                    var w2 = from kw in db.KeyWords where kw.Word.Trim().ToLower() == vw && kw.Type == 0 && (kw.GroupCommentId == null || kw.GroupCommentId == idgc) select new { Id = kw.Id };
                                    if (w2.ToList().Count <= 0)
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
                                        kw.TypeWord = 1;
                                        kw.C1 = 0;
                                        kw.C2 = 0;
                                        kw.C3 = 0;
                                        kw.C4 = 0;
                                        kw.C5 = 0;
                                        kw.P1 = 0;
                                        kw.P2 = 0;
                                        kw.P3 = 0;
                                        kw.P4 = 0;
                                        kw.P5 = 0;
                                        kw.Total = 0;
                                        kw.Logs = "Lần chạy:" + kc + " có C1=" + cc.C1;
                                        kw.GroupCommentId = idgc;
                                        db.KeyWords.Add(kw);
                                        db.SaveChanges();
                                    }
                                    //}
                                    //catch { }
                                }
                                else if (cc.C1 > 0)
                                {
                                    //Đưa các từ xuất hiện trong khía cạnh k vào TWk
                                    
                                    string vw = cc.Words.Trim().ToLower();
                                    var w2 = from kw in db.KeyWords where kw.Word.Trim().ToLower() == vw && kw.GroupWordId == gwid && (kw.GroupCommentId == null || kw.GroupCommentId == idgc) select new { Id = kw.Id };
                                    if (w2.ToList().Count <= 0)
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
                                        kw.Logs = "Lần chạy:" + kc + " có C1=" + cc.C1;
                                        kw.GroupCommentId = idgc;
                                        kw.TypeWord = 0;
                                        db.KeyWords.Add(kw);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    catch { }

                    #endregion

                    //var V2 = db.Vocabularies.Where(e => e.GroupCommentId == idgc && (e.TypeWord == "RB" || e.TypeWord == "RBR"
                    //    || e.TypeWord == "RBS" || e.TypeWord == "JJ" || e.TypeWord == "JJR" || e.TypeWord == "JJS")).ToList();
                    //foreach (var item in V2)
                    //{
                    //    string vw = item.Word.Trim().ToLower();
                    //    var w2 = from kw in db.KeyWords where kw.Word.Trim().ToLower() == vw && kw.GroupWordId == gwid && kw.TypeWord == 1 select new { Id = kw.Id};
                    //    if (w2.ToList().Count <= 0)
                    //    {
                    //        KeyWord kw = new KeyWord();
                    //        string kwid = Public.GetID();
                    //        while (db.KeyWords.Where(e => e.Id == kwid).ToList().Count > 0)
                    //        {
                    //            kwid = Public.GetID();
                    //        }
                    //        kw.Id = kwid;
                    //        kw.Word = vw;
                    //        kw.GroupWordId = gwid;
                    //        kw.Type = kkk;
                    //        kw.Logs = "Lần chạy:" + kc + "Thêm tính từ!";
                    //        kw.GroupCommentId = idgc;
                    //        kw.TypeWord = 1;
                    //        db.KeyWords.Add(kw);
                    //        db.SaveChanges();
                    //    }
                    //}
                }

                kkk++;

            }
            #endregion
        }

        private void AddLabels(string idgc, string idp)
        {
            int L = 3;
            
            var GC = db.GroupComents.Where(e => e.Id == idgc).ToList();

            foreach (var itemGC in GC)
            {
                int kc = 1;
                var S1 = db.Sentenses.Where(e => e.Comment.GroupCommentId == itemGC.Id).ToList();

                var gw = db.GroupWords.Where(e => e.ProductId == idp).ToList();
                #region Gán nhãn câu lần 1
                foreach (var itemS in S1)
                {
                    #region So khớp từ khóa
                    foreach (var itemGW in gw)
                    {
                        var w2 = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id && e.Type == 0).ToList();
                        //var w2 = from kw in db.KeyWords where kw.GroupWordId==itemGW.Id select new { Id = kw.Id, Word = kw.Word };
                        //int www = 0;
                        foreach (var itemW in w2)
                        {
                            try
                            {
                                string sN = "\\b" + Convert.ToString(itemW.Word) + "\\b";
                                Regex thegex = new Regex(sN.Trim().ToLower());
                                MatchCollection theMatches = thegex.Matches(itemS.ContentReview.Trim().ToLower());
                                if (theMatches.Count > 0)
                                {
                                    if (db.SeKeyWords.Where(e => e.KeyWordId == itemW.Id && e.SeId == itemS.Id).ToList().Count <= 0)
                                    {
                                        SeKeyWord skw2 = new SeKeyWord();
                                        string skwid = Public.GetID();
                                        while (db.SeKeyWords.Where(e => e.Id == skwid).ToList().Count > 0)
                                        {
                                            skwid = Public.GetID();
                                        }
                                        skw2.Id = skwid;
                                        skw2.KeyWordId = itemGW.Id;
                                        skw2.SeId = itemS.Id;
                                        skw2.CountNumber += "Chạy lần " + kc + ":" + itemW.Word + "/Khía cạnh:" + itemGW.Word;
                                        db.SeKeyWords.Add(skw2);
                                        
                                    }

                                    break;
                                }
                            }
                            catch { }
                        }
                        db.SaveChanges();
                    }
                    #endregion
                }
                #endregion
                UpdateKeyWord(gw, idgc, kc);
                kc++;
                while (kc <= L)
                {
                    try
                    {
                        #region Gán nhãn câu lần kc
                       
                        
                        foreach (var itemS in S1)
                        {
                            //itemS.ContentReview = "But a lot.";
                            #region Gán nhãn cho từng câu
                            bool k = true;
                            int mk = 0;

                            #region check câu đã được gán nhãn chưa
                            foreach (var itemGW in gw)
                            {
                                var w = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id && e.Type == 0 && (e.GroupCommentId == null || e.GroupCommentId == idgc)).ToList();
                                //var w = from kw in db.KeyWords where kw.GroupWordId == itemGW.Id && kw.Type == 0 && (kw.GroupCommentId == null || kw.GroupCommentId == idgc) select new { Id = kw.Id, Word = kw.Word };
                                //int www = 0;
                                foreach (var itemW in w)
                                {
                                    try
                                    {
                                        string sN = "\\b" + Convert.ToString(itemW.Word) + "\\b";
                                        Regex thegex = new Regex(sN.Trim().ToLower());
                                        MatchCollection theMatches = thegex.Matches(itemS.ContentReview.Trim().ToLower());

                                        if (theMatches.Count > 0)
                                        {
                                            if (db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.SeId == itemS.Id).ToList().Count <= 0)
                                            {
                                                SeKeyWord skw2 = new SeKeyWord();
                                                string skwid = Public.GetID();
                                                while (db.SeKeyWords.Where(e => e.Id == skwid).ToList().Count > 0)
                                                {
                                                    skwid = Public.GetID();
                                                }
                                                skw2.Id = skwid;
                                                skw2.KeyWordId = itemGW.Id;
                                                skw2.SeId = itemS.Id;
                                                skw2.CountNumber += "Chạy lần " + kc + ":" + itemW.Word + "/Khía cạnh:" + itemGW.Word;
                                                db.SeKeyWords.Add(skw2);
                                                db.SaveChanges();                                                                                     
                                            }
                                            k = false;
                                            break;                                           
                                        }
                                    }
                                    catch { }
                                    if ((k == false)) break;
                                }
                            }
                            #endregion

                            #region gán nhãn dựa vào TWk
                            if (k == true)
                            {
                                List<ListGroupKeyWord> listGW = new List<ListGroupKeyWord>();
                                string sl = itemS.ContentReview;
                                foreach (var itemGW in gw)
                                {
                                    sl += ". Khía cạnh "+ itemGW.Word+": ";
                                    mk = 0;
                                    //var w2 = from kw in db.KeyWords where kw.GroupWordId == itemGW.Id && (kw.Type == 0 || (kw.GroupCommentId == idgc && kw.TypeWord == 0)) select new { Id = kw.Id, Word = kw.Word };
                                    //var w = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id && (e.Type == 0 || (e.TypeWord == 0 && e.GroupCommentId == idgc))).ToList();
                                    var w2 = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id && (e.Type == 0 || (e.GroupCommentId == idgc && e.TypeWord == 0))).ToList();
                                    //int www = 0;
                                    foreach (var itemW in w2)
                                    {
                                        try
                                        {
                                            string sN = "\\b" + Convert.ToString(itemW.Word) + "\\b";
                                            Regex thegex = new Regex(sN.Trim().ToLower());
                                            MatchCollection theMatches = thegex.Matches(itemS.ContentReview.Trim().ToLower());

                                            if (theMatches.Count > 0)
                                            {
                                                mk++;
                                                //sl += itemW.Word + " ";
                                            }
                                        }
                                        catch { }
                                    }

                                    sl += mk+"; ";
                                    ListGroupKeyWord gkw = new ListGroupKeyWord();
                                    gkw.Id = itemGW.Id;
                                    gkw.Count = mk;
                                    gkw.Words = itemGW.Word;
                                    listGW.Add(gkw);
                                }


                                //listGW.Sort();
                                for (int i = 0; i < listGW.Count - 1; i++)
                                {
                                    for (int j = i + 1; j < listGW.Count; j++)
                                    {
                                        if (listGW[i].Count < listGW[j].Count)
                                        {
                                            ListGroupKeyWord tg = listGW[i];
                                            listGW[i] = listGW[j];
                                            listGW[j] = tg;
                                        }
                                    }
                                }

                                int Nmax = (int)listGW[0].Count;
                                if (Nmax > 0)
                                {
                                    foreach (var itemGW in listGW)
                                    {
                                        if (itemGW.Count == Nmax)
                                        {
                                            if (db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.SeId == itemS.Id).ToList().Count <= 0)
                                            {
                                                SeKeyWord skw2 = new SeKeyWord();
                                                string skwid = Public.GetID();
                                                while (db.SeKeyWords.Where(e => e.Id == skwid).ToList().Count > 0)
                                                {
                                                    skwid = Public.GetID();
                                                }
                                                skw2.Id = skwid;
                                                skw2.KeyWordId = itemGW.Id;
                                                skw2.SeId = itemS.Id;
                                                skw2.CountNumber += "Chạy lần " + kc + ":Số lượng từ TWk là " + itemGW.Count + " bao gồm các từ(" + sl + ")/Khía cạnh " + itemGW.Words;
                                                db.SeKeyWords.Add(skw2);
                                                db.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion

                            #endregion
                        }
                        #endregion
                        UpdateKeyWord(gw, idgc, kc);
                    }
                    catch { }
                    kc++;
                }

            }
        }

        private void ClearAddLebels(string idgc)
        {
            var skw = db.SeKeyWords.Where(e => e.Sentens.Comment.GroupCommentId == idgc).ToList();
            foreach (var itemSKW in skw)
            {
                var iskw= db.SeKeyWords.Find(itemSKW.Id);
                db.SeKeyWords.Remove(iskw);
                db.SaveChanges();
            }

            var kw = db.KeyWords.Where(e => e.GroupCommentId == idgc && e.TypeWord==0).ToList();
            foreach (var itemKW in kw)
            {
                var ikw = db.KeyWords.Find(itemKW.Id);
                db.KeyWords.Remove(ikw);
                db.SaveChanges();
            }
        }

        private void Statistics(string idp, string idgc)
        {
            var listGW = db.GroupWords.Where(e => e.ProductId == idp).ToList();
            //var listGW = db.GroupWords.ToList();
            foreach (var itemGW in listGW)
            {
                #region Thông kê điểm toàn tập review

                var listKW = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id && ((e.GroupCommentId == idgc && e.TypeWord == 1) || e.Type == 0)).ToList();
                //var listKW = from kw in db.KeyWords where kw.GroupWordId == itemGW.Id && ((kw.GroupCommentId == idgc && kw.TypeWord == 1) || kw.Type == 0) select new { Id = kw.Id, Word = kw.Word };
                //var listKW = db.KeyWords.Where(e => e.GroupWordId == "160421084754920" && e.Id == "170217120828244").ToList();

                var listCM = db.Comments.Where(e => e.GroupCommentId == idgc).ToList();
                foreach (var itemKW in listKW)
                {
                    foreach (var itemCM in listCM)
                    {
                        string sN = "\\b" + Convert.ToString(itemKW.Word) + "\\b";
                        Regex thegex = new Regex(sN.ToLower());
                        MatchCollection theMatches = thegex.Matches(itemCM.Comment1.ToLower());
                        if (theMatches.Count > 0)
                        {
                            var itemK = db.KeyWords.Where(e => e.Id == itemKW.Id).ToList();
                            //var itemK = (from kw in db.KeyWords where kw.Id == itemKW.Id
                            //             select new { Id = kw.Id, C1 = kw.C1, C2 = kw.C2, C3 = kw.C3, C4 = kw.C4, C5 = kw.C5,
                            //                          Total=kw.Total, P1 = kw.P1, P2 = kw.P2, P3 = kw.P3, P4 = kw.P4, P5 = kw.P5
                            //             }).AsEnumerable();

                            var kc = itemK.FirstOrDefault();

                            decimal rv = -1;

                            var score = db.GroupWordComments.Where(e => e.CommentId == itemCM.Id && e.GroupWordId == itemGW.Id).Select(e => e.Score).FirstOrDefault();
                            if (score != null)
                            {
                                rv = (decimal)score;
                            }

                            if (rv != -1)
                            {
                                kc.Total += theMatches.Count;

                                if (rv == 5)
                                {
                                    kc.C5 += theMatches.Count;
                                }
                                else if (rv == 4)
                                {
                                    kc.C4 += theMatches.Count;
                                }
                                else if (rv == 3)
                                {
                                    kc.C3 += theMatches.Count;
                                }
                                else if (rv == 2)
                                {
                                    kc.C2 += theMatches.Count;
                                }
                                else if (rv == 1)
                                {
                                    kc.C1 += theMatches.Count;
                                }

                                kc.P5 = (double)kc.C5 / (double)kc.Total;
                                kc.P4 = (double)kc.C4 / (double)kc.Total;
                                kc.P3 = (double)kc.C3 / (double)kc.Total;
                                kc.P2 = (double)kc.C2 / (double)kc.Total;
                                kc.P1 = (double)kc.C1 / (double)kc.Total;
                            }

                            db.Entry(kc).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                #endregion
            }

            foreach (var itemGW in listGW)
            {
                #region Thông kê trên từng tập review
                //là số các nhận xét có gán nhãn khía cạnh Ai được tính điểm Rk và tổng số nhận xét có gán nhãn khía cạnh Ai trong tập huấn luyện.
                int c1 = 0, c2 = 0, c3 = 0, c4 = 0, c5 = 0;

                var gw = db.GroupWords.Find(itemGW.Id);

                var data = (from gwc in db.GroupWordComments
                            join cmt in db.Comments on gwc.CommentId equals cmt.Id
                            join sen in db.Sentenses on cmt.Id equals sen.CommentId
                            join skw in db.SeKeyWords on sen.Id equals skw.SeId
                            select new
                            {
                                GroupWordId = gwc.GroupWordId,
                                Score = gwc.Score
                            }).AsEnumerable();

                var data2 = data.ToList();
                gw.C1 = c1 = data2.Where(e => e.GroupWordId == itemGW.Id && e.Score == 1).ToList().Count;
                gw.C2 = c2 = data2.Where(e => e.GroupWordId == itemGW.Id && e.Score == 2).ToList().Count;
                gw.C3 = c3 = data2.Where(e => e.GroupWordId == itemGW.Id && e.Score == 3).ToList().Count;
                gw.C4 = c4 = data2.Where(e => e.GroupWordId == itemGW.Id && e.Score == 4).ToList().Count;
                gw.C5 = c5 = data2.Where(e => e.GroupWordId == itemGW.Id && e.Score == 5).ToList().Count;

                gw.Total = c1 + c2 + c3 + c4 + c5;
                db.Entry(gw).State = EntityState.Modified;
                db.SaveChanges();

                //là số các nhận xét có gán nhãn Ai xuất hiện Fi được tính điểm Rk và số các nhận xét có gán nhãn khía cạnh Ai được tính điểm Rk
                var listC = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.GroupCommentId == idgc).ToList();
                if (listC.Count > 0)
                {
                    var kw = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id && ((e.GroupCommentId == idgc && e.TypeWord == 1) || e.Type == 0)).ToList();
                    foreach (var itemW in kw)
                    {
                        foreach (var itemC in listC)
                        {
                            string sN = "\\b" + Convert.ToString(itemW.Word) + "\\b";
                            Regex thegex = new Regex(sN.ToLower());
                            MatchCollection theMatches = thegex.Matches(itemC.Sentens.ContentReview.Trim().ToLower());
                            if (theMatches.Count > 0)
                            {
                                decimal rv = -1;
                                var score = db.GroupWordComments.Where(e => e.CommentId == itemC.Sentens.CommentId && e.GroupWordId == itemGW.Id).Select(e => e.Score).FirstOrDefault();
                                if (score != null)
                                {
                                    rv = (decimal)score;
                                }

                                if (rv != -1)
                                {
                                    var itemKC = db.KeywordsCounts.Where(e => e.GroupKeyWordId == itemGW.Id && e.KeyWord == itemW.Id).ToList();
                                    if (itemKC.Count <= 0)
                                    {
                                        KeywordsCount kc = new KeywordsCount();
                                        kc.GroupKeyWordId = itemGW.Id;
                                        kc.GroupCommentId = idgc;
                                        kc.KeyWord = itemW.Id;
                                        kc.Total = 1;

                                        if (rv == 5)
                                        {
                                            kc.C1 = 0;
                                            kc.C2 = 0;
                                            kc.C3 = 0;
                                            kc.C4 = 0;
                                            kc.C5 = 1;
                                            kc.P1 = 0;
                                            kc.P2 = 0;
                                            kc.P3 = 0;
                                            kc.P4 = 0;
                                            kc.P5 = 2 / (c5 + itemW.C5 + 1);
                                        }
                                        else if (rv == 4)
                                        {
                                            kc.C1 = 0;
                                            kc.C2 = 0;
                                            kc.C3 = 0;
                                            kc.C4 = 1;
                                            kc.C5 = 0;
                                            kc.P1 = 0;
                                            kc.P2 = 0;
                                            kc.P3 = 0;
                                            kc.P4 = 2 / (c4 + itemW.C4 + 1);
                                            kc.P5 = 0;
                                        }
                                        else if (rv == 3)
                                        {
                                            kc.C1 = 0;
                                            kc.C2 = 0;
                                            kc.C3 = 1;
                                            kc.C4 = 0;
                                            kc.C5 = 0;
                                            kc.P1 = 0;
                                            kc.P2 = 0;
                                            kc.P3 = 2 / (c3 + itemW.C3 + 1);
                                            kc.P4 = 0;
                                            kc.P5 = 0;
                                        }
                                        else if (rv == 2)
                                        {
                                            kc.C1 = 0;
                                            kc.C2 = 1;
                                            kc.C3 = 0;
                                            kc.C4 = 0;
                                            kc.C5 = 0;
                                            kc.P1 = 0;
                                            kc.P2 = 2 / (c2 + itemW.C2 + 1);
                                            kc.P3 = 0;
                                            kc.P4 = 0;
                                            kc.P5 = 0;
                                        }
                                        else if (rv == 1)
                                        {
                                            kc.C1 = 1;
                                            kc.C2 = 0;
                                            kc.C3 = 0;
                                            kc.C4 = 0;
                                            kc.C5 = 0;
                                            kc.P1 = 2 / (c1 + itemW.C1 + 1);
                                            kc.P2 = 0;
                                            kc.P3 = 0;
                                            kc.P4 = 0;
                                            kc.P5 = 0;
                                        }
                                        else
                                        {
                                            kc.C1 = 0;
                                            kc.C2 = 0;
                                            kc.C3 = 0;
                                            kc.C4 = 0;
                                            kc.C5 = 0;
                                            kc.P1 = (double)rv;
                                            kc.P2 = 0;
                                            kc.P3 = 0;
                                            kc.P4 = 0;
                                            kc.P5 = 0;
                                        }

                                        db.Entry(gw).State = EntityState.Modified;
                                        db.KeywordsCounts.Add(kc);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        var kc = itemKC.FirstOrDefault();
                                        kc.Total += 1;
                                        if (rv == 5)
                                        {
                                            kc.C5 += 1;
                                        }
                                        else if (rv == 4)
                                        {
                                            kc.C4 += 1;
                                        }
                                        else if (rv == 3)
                                        {
                                            kc.C3 += 1;
                                        }
                                        else if (rv == 2)
                                        {
                                            kc.C2 += 1;
                                        }
                                        else if (rv == 1)
                                        {
                                            kc.C1 += 1;
                                        }

                                        if (c5 != 0)
                                        {
                                            kc.P5 = ((double)kc.C5 + 1) / (c5 + itemW.C5 + 1);
                                        }
                                        if (c4 != 0)
                                        {
                                            kc.P4 = ((double)kc.C4 + 1) / (c4 + itemW.C4 + 1);
                                        }
                                        if (c3 != 0)
                                        {
                                            kc.P3 = ((double)kc.C3 + 1) / (c3 + itemW.C3 + 1);
                                        }
                                        if (c2 != 0)
                                        {
                                            kc.P2 = ((double)kc.C2 + 1) / (c2 + itemW.C2 + 1);
                                        }
                                        if (c1 != 0)
                                        {
                                            kc.P1 = ((double)kc.C1 + 1) / (c1 + itemW.C1 + 1);
                                        }

                                        db.Entry(kc).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }


                            }
                        }
                    }
                }
                #endregion
            }
        }

        private void Statistics2(string idp, string idgc)
        {
            var listGW = db.GroupWords.Where(e => e.ProductId == idp).ToList();
            foreach (var itemGW in listGW)
            {
                var listKW = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id && ((e.GroupCommentId == idgc && e.TypeWord == 1) || e.Type == 0)).ToList();
                var v = listKW.Sum(e => e.Total);
                foreach (var itemKW in listKW)
                {
                    //var kc = itemKW;

                    itemKW.P5 = (double)(itemKW.C5 + 1) / (double)(itemGW.C5 + 1 + v);
                    itemKW.P4 = (double)(itemKW.C4 + 1) / (double)(itemGW.C4 + 1 + v);
                    itemKW.P3 = (double)(itemKW.C3 + 1) / (double)(itemGW.C3 + 1 + v);
                    itemKW.P2 = (double)(itemKW.C2 + 1) / (double)(itemGW.C2 + 1 + v);
                    itemKW.P1 = (double)(itemKW.C1 + 1) / (double)(itemGW.C1 + 1 + v);

                    db.Entry(itemKW).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }
        }

        //public void AddLabels(string idgc)
        //{
        //    var GC = db.GroupComents.Where(e=>e.Id==idgc).ToList();
        //    foreach (var itemGC in GC)
        //    {
        //        int kc = 0;
        //        while (kc < 2)
        //        {
        //            try
        //            {
        //                var S1 = db.Sentenses.Where(e => e.Comment.GroupCommentId == itemGC.Id).ToList();
        //                #region Gán nhãn câu
        //                foreach (var itemS in S1)
        //                {
        //                    var gW = db.GroupWords.ToList();
        //                    List<ListCountKeyWord> countKW = new List<ListCountKeyWord>();
        //                    List<ListGroupKeyWord> countGW = new List<ListGroupKeyWord>();
        //                    #region So khớp từ khóa
        //                    foreach (var itemGW in gW)
        //                    {
        //                        var w = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id && (e.GroupCommentId==idgc || e.Type==0)).ToList();
        //                        int www = 0;
        //                        foreach (var itemW in w)
        //                        {
        //                            try
        //                            {
        //                                string sN = "\\b" + Convert.ToString(itemW.Word) + "\\b";
        //                                Regex thegex = new Regex(sN.Trim().ToLower());
        //                                MatchCollection theMatches = thegex.Matches(itemS.ContentReview.Trim().ToLower());
        //                                www += theMatches.Count;
        //                                if (theMatches.Count > 0)
        //                                {
        //                                    countKW.Add(new ListCountKeyWord { Count = theMatches.Count, Id = itemW.Id, Words = itemW.Word, GroupWords = itemGW.Word });
        //                                }
        //                            }
        //                            catch { }
        //                        }
        //                        countGW.Add(new ListGroupKeyWord { Id = itemGW.Id, Count = www, Words = itemGW.Word });
        //                    }
        //                    #endregion
        //                    #region Sắp xếp
        //                    try
        //                    {
        //                        if (countGW.Count > 0)
        //                        {
        //                            //Sắp xếp các countW
        //                            for (int i = 0; i < countGW.Count - 1; i++)
        //                            {
        //                                for (int j = i + 1; j < countGW.Count; j++)
        //                                {
        //                                    if (countGW[i].Count < countGW[j].Count)
        //                                    {
        //                                        ListGroupKeyWord tg = countGW[i];
        //                                        countGW[i] = countGW[j];
        //                                        countGW[j] = tg;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    catch { }
        //                    #endregion

        //                    #region Log gán nhãn
        //                    try
        //                    {
        //                        LogLabel log = new LogLabel();
        //                        string logid = Public.GetID();
        //                        while (db.LogLabels.Where(e => e.Id == logid).ToList().Count > 0)
        //                        {
        //                            logid = Public.GetID();
        //                        }
        //                        log.Id = logid;
        //                        log.ReviewContent = itemS.ContentReview;
        //                        string logcount = "";
        //                        string groupword = "";
        //                        foreach (var kws in countKW)
        //                        {
        //                            logcount += kws.Words + "/" + kws.Count + " | ";
        //                        }

        //                        foreach (var gkws in countGW)
        //                        {
        //                            if (gkws.Count > 0)
        //                            {
        //                                groupword += gkws.Words + "/" + gkws.Count + " | ";
        //                            }
        //                        }
        //                        log.LogCounts = logcount;
        //                        log.Steps += "Chạy lần " + kc;
        //                        log.GroupKeywords = groupword;
        //                        log.GroupCommentId = idgc;
        //                        db.LogLabels.Add(log);
        //                        db.SaveChanges();
        //                    }
        //                    catch { }

        //                    #endregion

        //                    #region Gán nhãn từ khóa
        //                    try
        //                    {
        //                        if (countGW.Count > 0)
        //                        {
        //                            if (countGW[0].Count > 0)
        //                            {
        //                                //Xóa câu đã được gán nhãn trc đó để gán nhãn lại
        //                                var skw3 = db.SeKeyWords.Where(e => e.SeId == itemS.Id).ToList();
        //                                foreach (var itemSKW3 in skw3)
        //                                {
        //                                    var skw4 = db.SeKeyWords.Find(itemSKW3.Id);
        //                                    db.SeKeyWords.Remove(skw4);
        //                                    db.SaveChanges();
        //                                }

        //                                for (int i = 0; i < countGW.Count; i++)
        //                                {
        //                                    if (countGW[i].Count == countGW[0].Count)
        //                                    {
        //                                        string skwi = countGW[i].Id;
        //                                        if (db.SeKeyWords.Where(e => e.KeyWordId == skwi && e.SeId == itemS.Id).ToList().Count <= 0)
        //                                        {
        //                                            SeKeyWord skw2 = new SeKeyWord();
        //                                            string skwid = Public.GetID();
        //                                            while (db.SeKeyWords.Where(e => e.Id == skwid).ToList().Count > 0)
        //                                            {
        //                                                skwid = Public.GetID();
        //                                            }
        //                                            skw2.Id = skwid;
        //                                            skw2.KeyWordId = countGW[i].Id;
        //                                            skw2.SeId = itemS.Id;
        //                                            skw2.CountNumber += "Chạy lần " + kc + ":" + countGW[i].Words + "/" + countGW[i].Count;                                       
        //                                            db.SeKeyWords.Add(skw2);
        //                                            db.SaveChanges();
        //                                        }
        //                                        else
        //                                        {
        //                                            var skw = db.SeKeyWords.Where(e => e.KeyWordId == skwi && e.SeId == itemS.Id).First();
        //                                            skw.KeyWordId = countGW[i].Id;
        //                                            skw.SeId = itemS.Id;
        //                                            skw.CountNumber += "Chạy lần " + kc + ":" + countGW[i].Words + "/" + countGW[i].Count;
        //                                            db.Entry(skw).State = EntityState.Modified;
        //                                            db.SaveChanges();
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    catch { }
        //                    #endregion
        //                }
        //                #endregion

        //                #region Tính x2
        //                var gw = db.GroupWords.ToList();
        //                int kkk = 1;
        //                foreach (var itemGW in gw)
        //                {
        //                    string gwid = itemGW.Id;

        //                    List<ListVocabulary> x2 = new List<ListVocabulary>();
        //                    int dd = 0;

        //                    var S2 = db.SeKeyWords.Where(e => e.KeyWordId == gwid && e.Sentens.Comment.GroupCommentId==idgc).ToList();
        //                    if (S2.Count > 0)
        //                    {
        //                        var V = db.Vocabularies.Where(e=>e.GroupCommentId==idgc).ToList();
        //                        #region Tính các giá trị
        //                        foreach (var itemV in V)
        //                        {
        //                            int c1 = 0;

        //                            //c1: là số lần xuất hiện của từ w trong câu thuộc về khía cạnh Ai.
        //                            try
        //                            {
        //                                foreach (var itemA in S2)
        //                                {
        //                                    string sW = "\\b" + Convert.ToString(itemV.Word) + "\\b";
        //                                    Regex thegex1 = new Regex(sW.Trim().ToLower());
        //                                    MatchCollection theMatches1 = thegex1.Matches(itemA.Sentens.ContentReview.Trim().ToLower());
        //                                    if (theMatches1.Count > 0)
        //                                    {
        //                                        c1 += theMatches1.Count;
        //                                    }
        //                                }

        //                                x2.Add(new ListVocabulary { Id = itemV.Id, C1 = c1, Words = itemV.Word });
        //                            }
        //                            catch { }

        //                            LogAddKeyWord kw = new LogAddKeyWord();
        //                            string laid = Public.GetID();
        //                            while (db.LogAddKeyWords.Where(e => e.Id == laid).ToList().Count > 0)
        //                            {
        //                                laid = Public.GetID();
        //                            }
        //                            kw.Id = laid;
        //                            kw.GroupWordId = gwid;
        //                            kw.Words = itemV.Word;
        //                            kw.Logs = "Lần chạy:" + kc + " có C1=" + c1;
        //                            kw.GroupCommentId = idgc;
        //                            db.LogAddKeyWords.Add(kw);
        //                            db.SaveChanges();

        //                            dd++;
        //                        }

        //                        #endregion
        //                        #region Sắp xếp C1
        //                        try
        //                        {
        //                            for (int ii = 0; ii < x2.Count - 1; ii++)
        //                            {
        //                                for (int j = ii + 1; j < x2.Count; j++)
        //                                {
        //                                    if (x2[ii].C1 < x2[j].C1)
        //                                    {
        //                                        ListVocabulary tg = x2[ii];
        //                                        x2[ii] = x2[j];
        //                                        x2[j] = tg;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        catch { }
        //                        #endregion
        //                        #region Đưa các từ có tần xuất cao nhất vào khía cạnh Tj
        //                        try
        //                        {
        //                            if (x2.Count > 0)
        //                            {
        //                                int N = int.Parse(x2[0].C1.ToString());
        //                                foreach (var cc in x2)
        //                                {
        //                                    if (cc.C1 > 0 && cc.C1 == N)
        //                                    {
        //                                        //try
        //                                        //{
        //                                        string vw = cc.Words.Trim().ToLower();
        //                                        if (db.KeyWords.Where(e => e.Word.Trim().ToLower() == vw && (e.GroupWordId == gwid || e.Type==0)).ToList().Count <= 0)
        //                                        {
        //                                            KeyWord kw = new KeyWord();
        //                                            string kwid = Public.GetID();
        //                                            while (db.KeyWords.Where(e => e.Id == kwid).ToList().Count > 0)
        //                                            {
        //                                                kwid = Public.GetID();
        //                                            }
        //                                            kw.Id = kwid;
        //                                            kw.Word = vw;
        //                                            kw.GroupWordId = gwid;
        //                                            kw.Type = 0;
        //                                            kw.Logs = "Lần chạy:" + kc + " có C1=" + cc.C1;
        //                                            kw.GroupCommentId = idgc;
        //                                            db.KeyWords.Add(kw);
        //                                            db.SaveChanges();
        //                                        }
        //                                        //}
        //                                        //catch { }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        catch { }

        //                        #endregion

        //                        #region Đưa các từ vào khía cạnh Ki
        //                        var v2 = db.Vocabularies.Where(e => (e.TypeWord == "RB" || e.TypeWord == "JJ") && e.GroupCommentId==idgc).ToList();

        //                        try
        //                        {
        //                            foreach (var itemV2 in v2)
        //                            {
        //                                try
        //                                {
        //                                    foreach (var itemA in S2)
        //                                    {
        //                                        string sW = "\\b" + Convert.ToString(itemV2.Word) + "\\b";
        //                                        Regex thegex1 = new Regex(sW.Trim().ToLower());
        //                                        MatchCollection theMatches1 = thegex1.Matches(itemA.Sentens.ContentReview.Trim().ToLower());
        //                                        if (theMatches1.Count > 0)
        //                                        {
        //                                            string vw = itemV2.Word.Trim().ToLower();
        //                                            if (db.KeyWords.Where(e => e.Word.Trim().ToLower() == vw && e.GroupWordId == gwid && (e.GroupCommentId==idgc || e.Type==0)).ToList().Count <= 0)
        //                                            {
        //                                                KeyWord kw = new KeyWord();
        //                                                string kwid = Public.GetID();
        //                                                while (db.KeyWords.Where(e => e.Id == kwid).ToList().Count > 0)
        //                                                {
        //                                                    kwid = Public.GetID();
        //                                                }
        //                                                kw.Id = kwid;
        //                                                kw.Word = vw;
        //                                                kw.GroupWordId = gwid;
        //                                                kw.Type = kkk;
        //                                                kw.Logs = "Lần chạy:" + kc;
        //                                                kw.GroupCommentId = idgc;
        //                                                db.KeyWords.Add(kw);
        //                                                db.SaveChanges();
        //                                            }
        //                                        }
        //                                    }

        //                                }
        //                                catch { }
        //                            }
        //                        }
        //                        catch { }
        //                        #endregion
        //                    }

        //                    kkk++;

        //                }
        //                #endregion

        //                //Kiểm tra cập nhật khía cạnh
        //                //var w2 = db.KeyWords.Where(e => e.GroupWordId == k).ToList();
        //                //if ((w2.Count - ww) >= 3)
        //                //{
        //                //    status = true;
        //                //}
        //                //else
        //                //{
        //                //    status = false;
        //                //}

        //            }
        //            catch { }
        //            kc++;
        //        }
        //    }
        //}

        // Thông kê đếm theo số lượng và công thức làm trơn
        //public ActionResult Index()
        //{
        //    #region Làm mới bảng thông kê
        //    var kcc = db.KeywordsCounts.ToList();
        //    foreach (var itemKCC in kcc)
        //    {
        //        db.KeywordsCounts.Where(e => e.GroupKeyWordId == itemKCC.GroupKeyWordId).FirstOrDefault();
        //        db.KeywordsCounts.Remove(itemKCC);
        //        db.SaveChanges();
        //    }

        //    var listKWW = db.KeyWords.ToList();
        //    foreach (var itemKW in listKWW)
        //    {
        //        var ikw = db.KeyWords.Find(itemKW.Id);
        //        ikw.C1 = 0;
        //        ikw.C2 = 0;
        //        ikw.C3 = 0;
        //        ikw.C4 = 0;
        //        ikw.C5 = 0;
        //        ikw.P1 = 0;
        //        ikw.P2 = 0;
        //        ikw.P3 = 0;
        //        ikw.P4 = 0;
        //        ikw.P5 = 0;
        //        ikw.Total = 0;

        //        db.Entry(ikw).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }

        //    var listGWW = db.GroupWords.ToList();
        //    foreach (var itemGW in listGWW)
        //    {
        //        var igw = db.GroupWords.Find(itemGW.Id);
        //        igw.C1 = 0;
        //        igw.C2 = 0;
        //        igw.C3 = 0;
        //        igw.C4 = 0;
        //        igw.C5 = 0;
        //        igw.Total = 0;

        //        db.Entry(igw).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    #endregion

        //    #region Thống kê
        //    var listGW = db.GroupWords.ToList();
        //    foreach (var itemGW in listGW)
        //    {
        //        #region Thông kê trên từng tập review
        //        //là số các nhận xét có gán nhãn khía cạnh Ai được tính điểm Rk và tổng số nhận xét có gán nhãn khía cạnh Ai trong tập huấn luyện.
        //        int c1 = 0, c2 = 0, c3 = 0, c4 = 0, c5 = 0;

        //        var gw = db.GroupWords.Find(itemGW.Id);
        //        if (itemGW.Id == "160421084754914")
        //        {
        //            gw.C1 = c1 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Rooms == 1).ToList().Count;
        //            gw.C2 = c2 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Rooms == 2).ToList().Count;
        //            gw.C3 = c3 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Rooms == 3).ToList().Count;
        //            gw.C4 = c4 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Rooms == 4).ToList().Count;
        //            gw.C5 = c5 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Rooms == 5).ToList().Count;
        //        }
        //        else if (itemGW.Id == "160421084754915")
        //        {
        //            gw.C1 = c1 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Location == 1).ToList().Count;
        //            gw.C2 = c2 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Location == 2).ToList().Count;
        //            gw.C3 = c3 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Location == 3).ToList().Count;
        //            gw.C4 = c4 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Location == 4).ToList().Count;
        //            gw.C5 = c5 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Location == 5).ToList().Count;
        //        }
        //        else if (itemGW.Id == "160421084754916")
        //        {
        //            gw.C1 = c1 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Cleanliness == 1).ToList().Count;
        //            gw.C2 = c2 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Cleanliness == 2).ToList().Count;
        //            gw.C3 = c3 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Cleanliness == 3).ToList().Count;
        //            gw.C4 = c4 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Cleanliness == 4).ToList().Count;
        //            gw.C5 = c5 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Cleanliness == 5).ToList().Count;
        //        }
        //        else if (itemGW.Id == "160421084754917")
        //        {
        //            gw.C1 = c1 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.CheckinFrontDesk == 1).ToList().Count;
        //            gw.C2 = c2 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.CheckinFrontDesk == 2).ToList().Count;
        //            gw.C3 = c3 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.CheckinFrontDesk == 3).ToList().Count;
        //            gw.C4 = c4 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.CheckinFrontDesk == 4).ToList().Count;
        //            gw.C5 = c5 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.CheckinFrontDesk == 5).ToList().Count;
        //        }
        //        else if (itemGW.Id == "160421084754918")
        //        {
        //            gw.C1 = c1 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Service == 1).ToList().Count;
        //            gw.C2 = c2 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Service == 2).ToList().Count;
        //            gw.C3 = c3 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Service == 3).ToList().Count;
        //            gw.C4 = c4 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Service == 4).ToList().Count;
        //            gw.C5 = c5 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Service == 5).ToList().Count;
        //        }
        //        else if (itemGW.Id == "160421084754919")
        //        {
        //            gw.C1 = c1 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Value == 1).ToList().Count;
        //            gw.C2 = c2 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Value == 2).ToList().Count;
        //            gw.C3 = c3 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Value == 3).ToList().Count;
        //            gw.C4 = c4 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Value == 4).ToList().Count;
        //            gw.C5 = c5 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Value == 5).ToList().Count;
        //        }
        //        else if (itemGW.Id == "160421084754920")
        //        {
        //            gw.C1 = c1 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.BusinessService == 1).ToList().Count;
        //            gw.C2 = c2 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.BusinessService == 2).ToList().Count;
        //            gw.C3 = c3 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.BusinessService == 3).ToList().Count;
        //            gw.C4 = c4 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.BusinessService == 4).ToList().Count;
        //            gw.C5 = c5 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.BusinessService == 5).ToList().Count;
        //        }

        //        gw.Total = c1 + c2 + c3 + c4 + c5;
        //        db.Entry(gw).State = EntityState.Modified;
        //        db.SaveChanges();

        //        //là số các nhận xét có gán nhãn Ai xuất hiện Fi được tính điểm Rk và số các nhận xét có gán nhãn khía cạnh Ai được tính điểm Rk

        //        var listC = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id).ToList();
        //        if (listC.Count > 0)
        //        {
        //            var kw = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id).ToList();
        //            foreach (var itemW in kw)
        //            {
        //                foreach (var itemC in listC)
        //                {
        //                    string sN = "\\b" + Convert.ToString(itemW.Word) + "\\b";
        //                    Regex thegex = new Regex(sN.ToLower());
        //                    MatchCollection theMatches = thegex.Matches(itemC.Sentens.ContentReview.Trim().ToLower());
        //                    if (theMatches.Count > 0)
        //                    {

        //                        gw.C1 = c1 = db.SeKeyWords.Where(e => e.KeyWordId == itemGW.Id && e.Sentens.Comment.Rooms == 1).ToList().Count;

        //                        decimal rv = decimal.Parse(itemC.Sentens.Comment.Rating.ToString());

        //                        var itemKC = db.KeywordsCounts.Where(e => e.GroupKeyWordId == itemGW.Id && e.KeyWord == itemW.Id).ToList();
        //                        if (itemKC.Count <= 0)
        //                        {
        //                            KeywordsCount kc = new KeywordsCount();
        //                            kc.GroupKeyWordId = itemGW.Id;
        //                            kc.KeyWord = itemW.Id;
        //                            kc.Total = theMatches.Count;

        //                            if (rv == 5)
        //                            {
        //                                kc.C1 = 0;
        //                                kc.C2 = 0;
        //                                kc.C3 = 0;
        //                                kc.C4 = 0;
        //                                kc.C5 = theMatches.Count;
        //                                kc.P1 = 0;
        //                                kc.P2 = 0;
        //                                kc.P3 = 0;
        //                                kc.P4 = 0;
        //                                if (c5 != 0)
        //                                {
        //                                    kc.P5 = theMatches.Count / c5;
        //                                }
        //                                else
        //                                {
        //                                    kc.P5 = 0;
        //                                }
        //                            }
        //                            else if (rv == 4)
        //                            {
        //                                kc.C1 = 0;
        //                                kc.C2 = 0;
        //                                kc.C3 = 0;
        //                                kc.C4 = theMatches.Count;
        //                                kc.C5 = 0;
        //                                kc.P1 = 0;
        //                                kc.P2 = 0;
        //                                kc.P3 = 0;
        //                                if (c4 != 0)
        //                                {
        //                                    kc.P4 = theMatches.Count / c4;
        //                                }
        //                                else
        //                                {
        //                                    kc.P4 = 0;
        //                                }
        //                                kc.P5 = 0;
        //                            }
        //                            else if (rv == 3)
        //                            {
        //                                kc.C1 = 0;
        //                                kc.C2 = 0;
        //                                kc.C3 = theMatches.Count;
        //                                kc.C4 = 0;
        //                                kc.C5 = 0;
        //                                kc.P1 = 0;
        //                                kc.P2 = 0;
        //                                if (c3 != 0)
        //                                {
        //                                    kc.P3 = theMatches.Count / c3;
        //                                }
        //                                else
        //                                {
        //                                    kc.P3 = 0;
        //                                }
        //                                kc.P4 = 0;
        //                                kc.P5 = 0;
        //                            }
        //                            else if (rv == 2)
        //                            {
        //                                kc.C1 = 0;
        //                                kc.C2 = theMatches.Count;
        //                                kc.C3 = 0;
        //                                kc.C4 = 0;
        //                                kc.C5 = 0;
        //                                kc.P1 = 0;
        //                                if (c2 != 0)
        //                                {
        //                                    kc.P2 = theMatches.Count / c2;
        //                                }
        //                                else
        //                                {
        //                                    kc.P2 = 0;
        //                                }
        //                                kc.P3 = 0;
        //                                kc.P4 = 0;
        //                                kc.P5 = 0;
        //                            }
        //                            else if (rv == 1)
        //                            {
        //                                kc.C1 = theMatches.Count;
        //                                kc.C2 = 0;
        //                                kc.C3 = 0;
        //                                kc.C4 = 0;
        //                                kc.C5 = 0;
        //                                if (c1 != 0)
        //                                {
        //                                    kc.P1 = theMatches.Count / c1;
        //                                }
        //                                else
        //                                {
        //                                    kc.P1 = 0;
        //                                }
        //                                kc.P2 = 0;
        //                                kc.P3 = 0;
        //                                kc.P4 = 0;
        //                                kc.P5 = 0;
        //                            }

        //                            db.Entry(gw).State = EntityState.Modified;
        //                            db.KeywordsCounts.Add(kc);
        //                            db.SaveChanges();
        //                        }
        //                        else
        //                        {
        //                            var kc = itemKC.FirstOrDefault();
        //                            kc.Total += theMatches.Count;
        //                            if (rv == 5)
        //                            {
        //                                kc.C5 += theMatches.Count;
        //                            }
        //                            else if (rv == 4)
        //                            {
        //                                kc.C4 += theMatches.Count;
        //                            }
        //                            else if (rv == 3)
        //                            {
        //                                kc.C3 += theMatches.Count;
        //                            }
        //                            else if (rv == 2)
        //                            {
        //                                kc.C2 += theMatches.Count;
        //                            }
        //                            else if (rv == 1)
        //                            {
        //                                kc.C1 += theMatches.Count;
        //                            }

        //                            if (c5 != 0)
        //                            {
        //                                kc.P5 = (decimal)kc.C5 / c5;
        //                            }
        //                            if (c4 != 0)
        //                            {
        //                                kc.P4 = (decimal)kc.C4 / c4;
        //                            }
        //                            if (c3 != 0)
        //                            {
        //                                kc.P3 = (decimal)kc.C3 / c3;
        //                            }
        //                            if (c2 != 0)
        //                            {
        //                                kc.P2 = (decimal)kc.C2 / c2;
        //                            }
        //                            if (c1 != 0)
        //                            {
        //                                kc.P1 = (decimal)kc.C1 / c1;
        //                            }

        //                            db.Entry(kc).State = EntityState.Modified;
        //                            db.SaveChanges();
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //        #endregion

        //        #region Thông kê điểm toàn tập review

        //        //int c11 = 0, c22 = 0, c33 = 0, c44 = 0, c55 = 0;
        //        //c11 = db.Sentenses.Where(e => e.Comment.Rating == 1).ToList().Count;
        //        //c22 = db.Sentenses.Where(e => e.Comment.Rating == 2).ToList().Count;
        //        //c33 = db.Sentenses.Where(e => e.Comment.Rating == 3).ToList().Count;
        //        //c44 = db.Sentenses.Where(e => e.Comment.Rating == 4).ToList().Count;
        //        //c55 = db.Sentenses.Where(e => e.Comment.Rating == 5).ToList().Count;

        //        var listKW = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id).ToList();

        //        var listCM = db.Comments.Where(e => e.GroupCommentId == "170208082946554").ToList();
        //        foreach (var itemKW in listKW)
        //        {
        //            foreach (var itemCM in listCM)
        //            {
        //                string sN = "\\b" + Convert.ToString(itemKW.Word) + "\\b";
        //                Regex thegex = new Regex(sN.ToLower());
        //                MatchCollection theMatches = thegex.Matches(itemCM.Comment1.ToLower());
        //                if (theMatches.Count > 0)
        //                {
        //                    var itemK = db.KeyWords.Where(e => e.Id == itemKW.Id).ToList();

        //                    var kc = itemK.FirstOrDefault();
        //                    kc.Total += theMatches.Count;
        //                    decimal rv = decimal.Parse(itemCM.Rating.ToString());
        //                    if (rv == 5)
        //                    {
        //                        kc.C5 += theMatches.Count;
        //                    }
        //                    else if (rv == 4)
        //                    {
        //                        kc.C4 += theMatches.Count;
        //                    }
        //                    else if (rv == 3)
        //                    {
        //                        kc.C3 += theMatches.Count;
        //                    }
        //                    else if (rv == 2)
        //                    {
        //                        kc.C2 += theMatches.Count;
        //                    }
        //                    else if (rv == 1)
        //                    {
        //                        kc.C1 += theMatches.Count;
        //                    }

        //                    kc.P5 = (decimal)kc.C5 / (decimal)kc.Total;
        //                    kc.P4 = (decimal)kc.C4 / (decimal)kc.Total;
        //                    kc.P3 = (decimal)kc.C3 / (decimal)kc.Total;
        //                    kc.P2 = (decimal)kc.C2 / (decimal)kc.Total;
        //                    kc.P1 = (decimal)kc.C1 / (decimal)kc.Total;

        //                    db.Entry(kc).State = EntityState.Modified;
        //                    db.SaveChanges();
        //                }
        //            }
        //        }
        //        #endregion
        //    }
        //    #endregion

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