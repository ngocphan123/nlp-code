using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyDoIT.Models.Common
{
    public class BeerJson
    {
        public string ReviewAppearance { get; set; }
        public string BeerStyle { get; set; }
        public string ReviewPalate { get; set; }
        public string ReviewTaste { get; set; }
        public string BeerName { get; set; }
        public string ReviewTimeUnix { get; set; }
        public string UserGender { get; set; }
        public string UserBirthdayRaw { get; set; }
        public string BeerABV { get; set; }
        public string BeerBeerId { get; set; }
        public string UserBirthdayUnix { get; set; }
        public string BeerBrewerId { get; set; }
        public IList<ReviewTimeStruct> ReviewTimeStruct { get; set; }
        public string UserAgeInSeconds { get; set; }
        public string ReviewOverall { get; set; }
        public string ReviewText { get; set; }
        public string UserProfileName { get; set; }
        public string ReviewAroma { get; set; }

    }
}