namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogLabel")]
    public partial class LogLabel
    {
        [StringLength(15)]
        public string Id { get; set; }

        [StringLength(50)]
        public string Steps { get; set; }

        public string ReviewContent { get; set; }

        public string LogCounts { get; set; }

        public string GroupKeywords { get; set; }

        [StringLength(15)]
        public string GroupCommentId { get; set; }

        public virtual GroupComent GroupComent { get; set; }
    }
}
