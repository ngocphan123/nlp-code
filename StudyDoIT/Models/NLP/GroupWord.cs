namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GroupWord
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GroupWord()
        {
            GroupWordComments = new HashSet<GroupWordComment>();
            KeywordsCounts = new HashSet<KeywordsCount>();
            LogAddKeyWords = new HashSet<LogAddKeyWord>();
            SeKeyWords = new HashSet<SeKeyWord>();
            KeyWords = new HashSet<KeyWord>();
        }

        [StringLength(15)]
        public string Id { get; set; }

        [StringLength(50)]
        public string Word { get; set; }

        public int? C1 { get; set; }

        public int? C2 { get; set; }

        public int? C3 { get; set; }

        public int? C4 { get; set; }

        public int? C5 { get; set; }

        public int? Total { get; set; }

        [StringLength(15)]
        public string ProductId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupWordComment> GroupWordComments { get; set; }

        public virtual Product Product { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KeywordsCount> KeywordsCounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogAddKeyWord> LogAddKeyWords { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SeKeyWord> SeKeyWords { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KeyWord> KeyWords { get; set; }
    }
}
