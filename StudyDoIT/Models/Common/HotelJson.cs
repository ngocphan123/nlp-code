using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyDoIT.Models.Common
{
    public class HotelJson
    {
        public IList<Reviews> Reviews { get; set; }

        public string HotelInfo { get; set; }
    }
}