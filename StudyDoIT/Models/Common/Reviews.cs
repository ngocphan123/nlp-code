using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyDoIT.Models.Common
{
    public class Reviews
    {
        public IList<Ratings> Ratings { get; set; }

        public string Content { get; set; }
        public string Date { get; set; }
        public string ReviewID { get; set; }
        public string Author { get; set; }
    }
}