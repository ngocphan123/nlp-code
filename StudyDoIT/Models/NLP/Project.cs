namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Project")]
    public partial class Project
    {
        [StringLength(15)]
        public string Id { get; set; }

        [StringLength(30)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Column(TypeName = "ntext")]
        public string Contents { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public DateTime? DatePublish { get; set; }

        public DateTime? DateUpdate { get; set; }

        public int? UserId { get; set; }

        public int? ViewNumber { get; set; }

        public int? LikeNumber { get; set; }

        public int? CommentNumber { get; set; }

        public int? Location { get; set; }

        public string Attactment { get; set; }

        public string Images { get; set; }

        public string Url { get; set; }

        public int? Hot { get; set; }

        public int? Publish { get; set; }

        [StringLength(200)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        public string MetaKeyword { get; set; }

        [StringLength(300)]
        public string MetaDescrption { get; set; }

        [Column(TypeName = "ntext")]
        public string ContentInfo { get; set; }

        [Column(TypeName = "ntext")]
        public string ContentPolicy { get; set; }

        [Column(TypeName = "ntext")]
        public string Promotion { get; set; }

        [StringLength(150)]
        public string Money { get; set; }

        public int? MoneyOld { get; set; }

        public int? MoneyPromotion { get; set; }

        public int? TypeImagePromotion { get; set; }

        public int? Type { get; set; }

        public virtual User User { get; set; }
    }
}
