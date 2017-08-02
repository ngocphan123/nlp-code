namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SeKeyWord")]
    public partial class SeKeyWord
    {
        [StringLength(15)]
        public string Id { get; set; }

        [StringLength(15)]
        public string SeId { get; set; }

        [StringLength(15)]
        public string KeyWordId { get; set; }

        [StringLength(500)]
        public string CountNumber { get; set; }

        public virtual GroupWord GroupWord { get; set; }

        public virtual Sentens Sentens { get; set; }
    }
}
