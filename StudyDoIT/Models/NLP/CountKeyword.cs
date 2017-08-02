namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CountKeyword")]
    public partial class CountKeyword
    {
        [StringLength(15)]
        public string Id { get; set; }

        [StringLength(15)]
        public string KeyWordId { get; set; }

        [StringLength(15)]
        public string GroupCommentId { get; set; }

        public int? Count { get; set; }

        public virtual GroupComent GroupComent { get; set; }

        public virtual KeyWord KeyWord { get; set; }
    }
}
