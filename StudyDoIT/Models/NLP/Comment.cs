namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Comment()
        {
            GroupWordComments = new HashSet<GroupWordComment>();
            LogVocabularies = new HashSet<LogVocabulary>();
            Sentenses = new HashSet<Sentens>();
        }

        [StringLength(15)]
        public string Id { get; set; }

        [StringLength(15)]
        public string GroupCommentId { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public decimal? Rating { get; set; }

        [Column("Comment")]
        public string Comment1 { get; set; }

        [StringLength(50)]
        public string Date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupWordComment> GroupWordComments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogVocabulary> LogVocabularies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sentens> Sentenses { get; set; }
    }
}
