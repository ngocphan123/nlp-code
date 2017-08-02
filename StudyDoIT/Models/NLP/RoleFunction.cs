namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoleFunction")]
    public partial class RoleFunction
    {
        [StringLength(15)]
        public string Id { get; set; }

        [StringLength(15)]
        public string RoleId { get; set; }

        public int? CategoryId { get; set; }

        public int? Location { get; set; }

        [StringLength(15)]
        public string FunctionParent { get; set; }

        public DateTime? DatePublish { get; set; }

        public int? UserId { get; set; }

        public virtual Category Category { get; set; }

        public virtual Role Role { get; set; }
    }
}
