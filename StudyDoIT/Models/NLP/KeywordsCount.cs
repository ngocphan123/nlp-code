namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KeywordsCount")]
    public partial class KeywordsCount
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string GroupKeyWordId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string KeyWord { get; set; }

        public int? C1 { get; set; }

        public int? C2 { get; set; }

        public int? C3 { get; set; }

        public int? C4 { get; set; }

        public int? C5 { get; set; }

        public double? P1 { get; set; }

        public double? P2 { get; set; }

        public double? P3 { get; set; }

        public double? P4 { get; set; }

        public double? P5 { get; set; }

        public int? Total { get; set; }

        [StringLength(15)]
        public string GroupCommentId { get; set; }

        public virtual GroupComent GroupComent { get; set; }

        public virtual GroupWord GroupWord { get; set; }

        public virtual KeyWord KeyWord1 { get; set; }
    }
}
