using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyDoIT.Models.NLP;
using StudyDoIT.Models.Common;
using System.Text.RegularExpressions;
using System.IO;

namespace StudyDoIT.Areas.AdminIT.Controllers
{
     [Authorize(Roles = "Administrator, Manager")]
    public class TestController : Controller
    {
         lCMSData db = new lCMSData();

         public class ListSeKeyWord
         {
             public int Id { get; set; }
             public string Comment { get; set; }
             public string Keyword { get; set; }
             public string GroupKeyword { get; set; }
         }

        //
        // GET: /AdminIT/Test/
        public ActionResult Index()
        {
            //Xủ lý phiên làm việc
            Session["current_url"] = Request.Url.AbsoluteUri;

            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            string review = collection["Input"];
            string resuft = "";
            string idp = "170329112813869";           
            #region  Tính điểm review cho từng khía cạnh
            resuft += reviewEveryOne(idp, review);
            #endregion

            #region Tính điểm review tổng thể
            //resuft += reviewAll(review);

            #endregion

            ViewBag.Resuft = resuft;

            return View();
        }

        private string reviewEveryOne(string idp, string review)
        {
            var listGW = db.GroupWords.Where(e => e.ProductId == idp).ToList();
            int[] pmax = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            string resuft="";
            int k = 0;
            foreach (var itemGW in listGW)
            {
                var listKW = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id && (e.TypeWord == 1 || e.Type==0)).ToList();
                double[] pi = new double[5] { 1.0, 1.0, 1.0, 1.0, 1.0 };
                
                //double pk = 0;
                //int total = 0;
                bool status = false;

                //foreach (var itemKW in listKW)
                //{
                //    total += (int)itemKW.Total;
                //}

                //if (total>0)
                //{
                bool k1 = false, k2 = false, k3 = false, k4 = false, k5 = false;
                foreach (var itemKW in listKW)
                {
                    string sN = "\\b" + Convert.ToString(itemKW.Word) + "\\b";
                    Regex thegex = new Regex(sN.ToLower());
                    MatchCollection theMatches = thegex.Matches(review.ToLower());
                    if (theMatches.Count > 0)
                    {
                        status = true;
                        //var kc = db.KeywordsCounts.Where(e => e.KeyWord == itemKW.Id && e.GroupKeyWordId == itemKW.GroupWordId).FirstOrDefault();
                        //var kw = db.KeyWords.Where(e => e.Word == itemKW.Id && e.GroupWordId == itemKW.GroupWordId).FirstOrDefault();
                        //total += (int)itemKW.Total;
                        if (itemKW != null)
                        {
                            if (itemKW.P1 != 0)
                            {
                                pi[0] *= Math.Pow((double)itemKW.P1, (double)theMatches.Count);
                                k1 = true;
                            }
                            if (itemKW.P2 != 0)
                            {
                                pi[1] *= Math.Pow((double)itemKW.P2, (double)theMatches.Count);
                                k2 = true;
                            }
                            if (itemKW.P3 != 0)
                            {
                                pi[2] *= Math.Pow((double)itemKW.P3, (double)theMatches.Count);
                                k3 = true;
                            }
                            if (itemKW.P4 != 0)
                            {
                                pi[3] *= Math.Pow((double)itemKW.P4, (double)theMatches.Count);
                                k4 = true;
                            }
                            if (itemKW.P5 != 0)
                            {
                                pi[4] *= Math.Pow((double)itemKW.P5, (double)theMatches.Count);
                                k5 = true;
                            }
                            //if (total != 0)
                            //{
                            //    pk += ((double)itemKW.Total / (double)total) * (double)theMatches.Count;
                            //}
                        }
                    }
                }

                if (k1 == false)
                {
                    pi[0] = 0;
                }
                if (k2 == false)
                {
                    pi[1] = 0;
                }
                if (k3 == false)
                {
                    pi[2] = 0;
                }
                if (k4 == false)
                {
                    pi[3] = 0;
                }
                if (k5 == false)
                {
                    pi[4] = 0;
                }

                if (status)
                {
                    //var gw = db.GroupWords.Find(itemGW.Id);
                    if (itemGW.Total != 0)
                    {
                        //Ko dùng Log
                        pi[0] = pi[0] * ((double)itemGW.C1 / (double)itemGW.Total);
                        pi[1] = pi[1] * ((double)itemGW.C2 / (double)itemGW.Total);
                        pi[2] = pi[2] * ((double)itemGW.C3 / (double)itemGW.Total);
                        pi[3] = pi[3] * ((double)itemGW.C4 / (double)itemGW.Total);
                        pi[4] = pi[4] * ((double)itemGW.C5 / (double)itemGW.Total);

                        //Dùng Log
                        //pi[0] = pi[0] * ((double)gw.C1 / (double)gw.Total) / pk;
                        //if (pi[0] != 0) pi[0] = Math.Log(pi[0]);
                        //pi[1] = pi[1] * ((double)gw.C2 / (double)gw.Total) / pk;
                        //if (pi[1] != 0) pi[1] = Math.Log(pi[1]);
                        //pi[2] = pi[2] * ((double)gw.C3 / (double)gw.Total) / pk;
                        //if (pi[2] != 0) pi[2] = Math.Log(pi[2]);
                        //pi[3] = pi[3] * ((double)gw.C4 / (double)gw.Total) / pk;
                        //if (pi[3] != 0) pi[3] = Math.Log(pi[3]);
                        //pi[4] = pi[4] * ((double)gw.C5 / (double)gw.Total) / pk;
                        //if (pi[4] != 0) pi[4] = Math.Log(pi[4]);
                    }


                    double max = 0.0;
                    if (pi[0] == 0)
                    {
                        pmax[k] = -1;
                    }
                    else
                    {
                        pmax[k] = 1;
                    }
                    int m = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (pi[i] != 0)
                        {
                            pmax[k] = i + 1;
                            max = pi[i];
                            m = i;
                            break;
                        }
                    }

                    for (int i = m; i < 5; i++)
                    {
                        if (pi[i] >= max && pi[i] != 0)
                        {
                            pmax[k] = i + 1;
                            max = pi[i];
                        }
                    }

                    resuft += "<ul><span style='margin-left: -50px;'><b> Khía cạnh " + itemGW.Word + " có: </b></span><li> p1=" + pi[0] + ",</li> <li>p2=" + pi[1]
                            + ",</li><li> p3=" + pi[2] + ",</li> <li> p4=" + pi[3] + ",</li><li> p5=" + pi[4]
                            + ",</li> <li> <b>=> được đánh giá " + pmax[k] + " điểm. </b></li></ul> ";
                }

                k++;
            }
            return resuft;
        }

        private string reviewAll(string review)
        {
            var listKWW = db.KeyWords.ToList();
            int[] pit = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            int[] pmax = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            string resuft = "";
            int totalt = 0;

            //foreach (var itemKWW in listKWW)
            //{
            //    totalt += (int)itemKWW.Total;
            //}

            //bool kt1 = false, kt2 = false, kt3 = false, kt4 = false, kt5 = false;
            foreach (var itemKWW in listKWW)
            {
                //totalt += (int)itemKWW.Total;
                string sN = "\\b" + Convert.ToString(itemKWW.Word) + "\\b";
                Regex thegex = new Regex(sN.ToLower());
                MatchCollection theMatches = thegex.Matches(review.ToLower());
                if (theMatches.Count > 0)
                {
                    //var kc = db.KeyWords.Find(itemKWW.Id);                    
                    if (itemKWW.GroupWordId == "160421084754914")
                    {
                        pit[0] += theMatches.Count;
                        if (pmax[0] != -1)
                        {
                            totalt += theMatches.Count;
                        }
                    }
                    else if (itemKWW.GroupWordId == "160421084754915")
                    {
                        pit[1] += theMatches.Count;
                        if (pmax[1] != -1)
                        {
                            totalt += theMatches.Count;
                        }
                    }
                    else if (itemKWW.GroupWordId == "160421084754916")
                    {
                        pit[2] += theMatches.Count;
                        if (pmax[2] != -1)
                        {
                            totalt += theMatches.Count;
                        }
                    }
                    else if (itemKWW.GroupWordId == "160421084754917")
                    {
                        pit[3] += theMatches.Count;
                        if (pmax[3] != -1)
                        {
                            totalt += theMatches.Count;
                        }
                    }
                    else if (itemKWW.GroupWordId == "160421084754918")
                    {
                        pit[4] += theMatches.Count;
                        if (pmax[4] != -1)
                        {
                            totalt += theMatches.Count;
                        }
                    }
                    else if (itemKWW.GroupWordId == "160421084754919")
                    {
                        pit[5] += theMatches.Count;
                        if (pmax[5] != -1)
                        {
                            totalt += theMatches.Count;
                        }
                    }
                    else if (itemKWW.GroupWordId == "160421084754920")
                    {
                        pit[6] += theMatches.Count;
                        if (pmax[6] != -1)
                        {
                            totalt += theMatches.Count;
                        }
                    }
                }
            }

            double pp = 0;
            //int kk = 0;
            for (int i = 0; i < 7; i++)
            {
                if (pmax[i] != -1)
                {
                    pp += ((double)pit[i] / (double)totalt) * pmax[i];
                }
            }
            //pp = (double)pp / (double)kk;

            resuft += "Đánh giá chung là " + Math.Round(pp, 0) + " điểm. \n ";
            return resuft;
        }

        //[HttpPost]
        //public ActionResult Index(FormCollection collection)
        //{
        //    string review = collection["Input"];
        //    string resuft = "";

        //    #region  Tính điểm review cho từng khía cạnh
        //    var listGW = db.GroupWords.ToList();
        //    foreach (var itemGW in listGW)
        //    {
        //        var listKW = db.KeyWords.Where(e => e.GroupWordId == itemGW.Id).ToList();
        //        double[] pi = new double[5] { 1.0, 1.0, 1.0, 1.0, 1.0 };
        //        double pk = 1;
        //        int total = 0;
        //        bool status = false;

        //        var listKC = db.KeywordsCounts.Where(e => e.GroupKeyWordId == itemGW.Id).ToList();
        //        foreach (var itemKC in listKC)
        //        {
        //            total += (int)itemKC.Total;
        //        }

        //        //if (total>0)
        //        //{
        //            bool k1 = false, k2 = false, k3 = false, k4 = false, k5 = false;
        //            foreach (var itemKW in listKW)
        //            {
        //                string sN = "\\b" + Convert.ToString(itemKW.Word) + "\\b";
        //                Regex thegex = new Regex(sN.ToLower());
        //                MatchCollection theMatches = thegex.Matches(review.ToLower());
        //                if (theMatches.Count > 0)
        //                {
        //                    status = true;
        //                    var kc = db.KeywordsCounts.Where(e => e.KeyWord == itemKW.Id && e.GroupKeyWordId == itemKW.GroupWordId).FirstOrDefault();
        //                    if (kc != null)
        //                    {
        //                        if (kc.Total != 0)
        //                        {
        //                            if (kc.P1 != 0)
        //                            {
        //                                pi[0] *= Math.Pow((double)kc.P1, (double)theMatches.Count);
        //                                k1 = true;
        //                            }
        //                            if (kc.P2 != 0)
        //                            {
        //                                pi[1] *= Math.Pow((double)kc.P2, (double)theMatches.Count);
        //                                k2 = true;
        //                            }
        //                            if (kc.P3 != 0)
        //                            {
        //                                pi[2] *= Math.Pow((double)kc.P3, (double)theMatches.Count);
        //                                k3 = true;
        //                            }
        //                            if (kc.P4 != 0)
        //                            {
        //                                pi[3] *= Math.Pow((double)kc.P4, (double)theMatches.Count);
        //                                k4 = true;
        //                            }
        //                            if (kc.P5 != 0)
        //                            {
        //                                pi[4] *= Math.Pow((double)kc.P5, (double)theMatches.Count);
        //                                k5 = true;
        //                            }
        //                            if (total != 0)
        //                            {
        //                                pk *= Math.Pow(((double)kc.Total / (double)total), (double)theMatches.Count);
        //                            }
        //                        }
        //                    }

        //                }
        //            }


        //            if (k1 == false)
        //            {
        //                pi[0] = 0;
        //            }
        //            if (k2 == false)
        //            {
        //                pi[1] = 0;
        //            }
        //            if (k3 == false)
        //            {
        //                pi[2] = 0;
        //            }
        //            if (k4 == false)
        //            {
        //                pi[3] = 0;
        //            }
        //            if (k5 == false)
        //            {
        //                pi[4] = 0;
        //            }

        //            if (status)
        //            {

        //                    var gw = db.GroupWords.Find(itemGW.Id);
        //                    if (gw.Total != 0)
        //                    {
        //                        pi[0] = (double)((pi[0] * (gw.C1 / gw.Total)) / pk);
        //                        pi[1] = (double)((pi[1] * (gw.C2 / gw.Total)) / pk);
        //                        pi[2] = (pi[2] * ((double)gw.C3 / (double)gw.Total)) / pk;
        //                        pi[3] = (pi[3] * ((double)gw.C4 / (double)gw.Total)) / pk;
        //                        pi[4] = (pi[4] * ((double)gw.C5 / (double)gw.Total)) / pk;
        //                    }

        //                int k = 0;
        //                double max = pi[0];
        //                for (int i = 1; i < 5; i++)
        //                {
        //                    if (pi[i] >= max)
        //                    {
        //                        k = i + 1;
        //                        max = pi[i];
        //                    }
        //                }

        //                resuft += "Khía cạnh " + itemGW.Word + " có p1=" + pi[0] + ", p2=" + pi[1]
        //                        + ", p3=" + pi[2] + ", p4=" + pi[3] + ", p5=" + pi[4]
        //                        + " => được đánh giá " + k + " điểm. \n ";
        //            //}
        //        }

        //    }
        //    #endregion

        //    #region Tính điểm review tổng thể
        //    var listKWW = db.KeyWords.ToList();
        //    double[] pit = new double[5] { 1.0, 1.0, 1.0, 1.0, 1.0 };
        //    double pkt = 1;
        //    int totalt = 0;

        //    foreach (var itemKWW in listKWW)
        //    {
        //        totalt += (int)itemKWW.Total;
        //    }

        //    bool kt1 = false, kt2 = false, kt3 = false, kt4 = false, kt5 = false;
        //    foreach (var itemKWW in listKWW)
        //    {
        //        //totalt += (int)itemKWW.Total;
        //        string sN = "\\b" + Convert.ToString(itemKWW.Word) + "\\b";
        //        Regex thegex = new Regex(sN.ToLower());
        //        MatchCollection theMatches = thegex.Matches(review.ToLower());
        //        if (theMatches.Count > 0)
        //        {
        //            var kc = db.KeyWords.Find(itemKWW.Id);
        //            if (kc.Total != 0)
        //            {
        //                if (kc.P1 != 0)
        //                {
        //                    pit[0] *= Math.Pow((double)kc.P1, (double)theMatches.Count);
        //                    kt1 = true;
        //                }
        //                if (kc.P2 != 0)
        //                {
        //                    pit[1] *= Math.Pow((double)kc.P2, (double)theMatches.Count);
        //                    kt2 = true;
        //                }
        //                if (kc.P3 != 0)
        //                {
        //                    pit[2] *= Math.Pow((double)kc.P3, (double)theMatches.Count);
        //                    kt3 = true;
        //                }
        //                if (kc.P4 != 0)
        //                {
        //                    pit[3] *= Math.Pow((double)kc.P4, (double)theMatches.Count);
        //                    kt4 = true;
        //                }
        //                if (kc.P5 != 0)
        //                {
        //                    pit[4] *= Math.Pow((double)kc.P5, (double)theMatches.Count);
        //                    kt5 = true;
        //                }
        //                if (totalt != 0)
        //                {
        //                    pkt *= Math.Pow(((double)kc.Total / (double)totalt), (double)theMatches.Count);
        //                }
        //            }
        //        }
        //    }

        //    if (kt1 == false)
        //    {
        //        pit[0] = 0;
        //    }
        //    if (kt2 == false)
        //    {
        //        pit[1] = 0;
        //    }
        //    if (kt3 == false)
        //    {
        //        pit[2] = 0;
        //    }
        //    if (kt4 == false)
        //    {
        //        pit[3] = 0;
        //    }
        //    if (kt5 == false)
        //    {
        //        pit[4] = 0;
        //    }

        //    var gww = db.GroupWords.ToList();
        //    int c1 = 0, c2 = 0, c3 = 0, c4 = 0, c5 = 0, t = 0;
        //    foreach (var itemGWW in gww)
        //    {
        //        c1 += (int)itemGWW.C1;
        //        c2 += (int)itemGWW.C2;
        //        c3 += (int)itemGWW.C3;
        //        c4 += (int)itemGWW.C4;
        //        c5 += (int)itemGWW.C5;
        //        t += (int)itemGWW.Total;
        //    }

        //    pit[0] = (pit[0] * ((double)c1 / (double)t)) / pkt;
        //    pit[1] = (pit[1] * ((double)c2 / (double)t)) / pkt;
        //    pit[2] = (pit[2] * ((double)c3 / (double)t)) / pkt;
        //    pit[3] = (pit[3] * ((double)c4 / (double)t)) / pkt;
        //    pit[4] = (pit[4] * ((double)c5 / (double)t)) / pkt;
        //    int kt = 1;
        //    double maxt = pit[0];
        //    for (int i = 1; i < 5; i++)
        //    {
        //        if (pit[i] >= maxt)
        //        {
        //            kt = i + 1;
        //            maxt = pit[i];
        //        }
        //    }

        //    resuft += "Đánh giá chung "
        //        + " có p1=" + pit[0] + ", p2=" + pit[1]
        //        + ", p3=" + pit[2] + ", p4=" + pit[3] + ", p5=" + pit[4]
        //        + " => được "+ kt + " điểm. \n ";

        //    #endregion

        //    ViewBag.Resuft = resuft;

        //    return View();
        //}

        public ActionResult List()
        {
            Session["current_url"] = Request.Url.AbsoluteUri;
            if (Session["Ad_TenDangNhap"] == null)
                return RedirectToAction("Login", "Account", null);

            var model = db.KeyWords.Where(e => e.Type == 0).Take(10).OrderByDescending(e => e.Id);
            return View(model);
        }

        //
        // GET: /AdminIT/Test/Details/5
        public ActionResult Details()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult LoadList(string comment)
        {
            try
            {
                //var listC = db.Comments.ToList();
                //string comment= collection["Description"].Trim();
                string[] listC=comment.Split('.', '!', '?');

                List<ListSeKeyWord> lskw = new List<ListSeKeyWord>();
                decimal score = 0;

                foreach (var itemC in listC)
                {
                    var kw = db.KeyWords.ToList();

                    foreach (var itemW in kw)
                    {
                        string sN = Convert.ToString(itemW.Word);
                        Regex thegex = new Regex(sN.ToLower());
                        MatchCollection theMatches = thegex.Matches(itemC);
                        if (theMatches.Count > 0)
                        {
                            var lkw = db.KeyWords.Where(e => e.Id == itemW.Id);
                            
                            foreach (var itemKW in lkw)
                            {
                                if(itemKW.Score!=null){
                                    score += decimal.Parse(itemKW.Score.ToString());
                                }
                                lskw.Add(new ListSeKeyWord { Id = lskw.Count+1, Comment = itemC, Keyword=itemKW.Word,GroupKeyword=itemKW.GroupWord.Word });
                            }
                        }
                    }
                }
                //var listKW = lskw.ToList();
                decimal scoreAVG = score / lskw.Count;
                ViewBag.Score = Math.Round(scoreAVG,1);
                return PartialView("_List", lskw);
                //return RedirectToAction("List", lskw);
            }
            catch
            {
                return View();
            }
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
