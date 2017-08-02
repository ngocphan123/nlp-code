namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KeyWord
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KeyWord()
        {
            CountKeywords = new HashSet<CountKeyword>();
            KeywordsCounts = new HashSet<KeywordsCount>();
        }

        [StringLength(15)]
        public string Id { get; set; }

        [StringLength(50)]
        public string Word { get; set; }

        [StringLength(15)]
        public string GroupWordId { get; set; }

        public int? Type { get; set; }

        public decimal? Score { get; set; }

        [StringLength(500)]
        public string Logs { get; set; }

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

        public int? TypeWord { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CountKeyword> CountKeywords { get; set; }

        public virtual GroupComent GroupComent { get; set; }

        public virtual GroupWord GroupWord { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KeywordsCount> KeywordsCounts { get; set; }
    }
}
