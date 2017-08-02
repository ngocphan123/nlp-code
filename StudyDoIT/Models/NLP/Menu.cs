namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        [StringLength(15)]
        public string Id { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(15)]
        public string TypeMenuId { get; set; }

        public int? Location { get; set; }

        [StringLength(15)]
        public string MenuParent { get; set; }

        public DateTime? DatePublish { get; set; }

        public DateTime? DateUpdate { get; set; }

        public int? UserId { get; set; }

        public int? Publish { get; set; }

        public virtual Category Category { get; set; }

        public virtual TypeMenu TypeMenu { get; set; }
    }
}
