namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoleUser
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string Role_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int User_Id { get; set; }

        public DateTime? DatePublish { get; set; }

        public int? UserCreate { get; set; }
    }
}
