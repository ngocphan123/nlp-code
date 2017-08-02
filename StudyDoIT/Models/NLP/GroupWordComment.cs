namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GroupWordComment")]
    public partial class GroupWordComment
    {
        [StringLength(15)]
        public string Id { get; set; }

        [StringLength(15)]
        public string GroupWordId { get; set; }

        [StringLength(15)]
        public string CommentId { get; set; }

        public double Score { get; set; }

        public virtual Comment Comment { get; set; }

        public virtual GroupWord GroupWord { get; set; }
    }
}
