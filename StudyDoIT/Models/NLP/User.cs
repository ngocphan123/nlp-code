namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Categories = new HashSet<Category>();
            Contacts = new HashSet<Contact>();
            EducationJobs = new HashSet<EducationJob>();
            HobbiesInterests = new HashSet<HobbiesInterest>();
            MySpecialities = new HashSet<MySpeciality>();
            Projects = new HashSet<Project>();
            SkillsAbilities = new HashSet<SkillsAbility>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string UserName { get; set; }

        [Required]
        [StringLength(200)]
        public string PasswordHash { get; set; }

        public string FullName { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(15)]
        public string RoleId { get; set; }

        public bool? EmailConfirmaed { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        public DateTime Birthday { get; set; }

        public string Address { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        public string Jobs { get; set; }

        public string Attactment { get; set; }

        public string SecurityStamp { get; set; }

        public string Discriminator { get; set; }

        public string Cover { get; set; }

        public string Avata { get; set; }

        public bool? PhoneNumberConfirmed { get; set; }

        public DateTime LockoutEndDateUtc { get; set; }

        public bool? LockoutEnabled { get; set; }

        public bool? Lock { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contacts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EducationJob> EducationJobs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HobbiesInterest> HobbiesInterests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MySpeciality> MySpecialities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> Projects { get; set; }

        public virtual Role Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SkillsAbility> SkillsAbilities { get; set; }
    }
}
