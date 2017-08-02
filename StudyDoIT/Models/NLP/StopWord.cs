namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StopWord
    {
        [StringLength(15)]
        public string Id { get; set; }

        [Column("StopWord")]
        [StringLength(50)]
        public string StopWord1 { get; set; }
    }
}
