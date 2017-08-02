namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogVocabulary")]
    public partial class LogVocabulary
    {
        [StringLength(15)]
        public string Id { get; set; }

        [StringLength(15)]
        public string CommentId { get; set; }

        public DateTime? CreateAt { get; set; }

        public string Logs { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
