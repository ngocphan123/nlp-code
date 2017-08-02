namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogAddKeyWord")]
    public partial class LogAddKeyWord
    {
        [StringLength(15)]
        public string Id { get; set; }

        [StringLength(15)]
        public string GroupWordId { get; set; }

        [StringLength(50)]
        public string Words { get; set; }

        public string Logs { get; set; }

        [StringLength(15)]
        public string GroupCommentId { get; set; }

        public virtual GroupComent GroupComent { get; set; }

        public virtual GroupWord GroupWord { get; set; }
    }
}
