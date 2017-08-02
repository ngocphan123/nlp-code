namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MySpeciality
    {
        [StringLength(15)]
        public string Id { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Contents { get; set; }

        [StringLength(500)]
        public string Icon { get; set; }

        public int? UserId { get; set; }

        public bool? Publish { get; set; }

        public virtual User User { get; set; }
    }
}
