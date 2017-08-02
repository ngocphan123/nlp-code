namespace StudyDoIT.Models.NLP
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class lCMSData : DbContext
    {
        public lCMSData()
            : base("name=lCMSData1")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<CountKeyword> CountKeywords { get; set; }
        public virtual DbSet<EducationJob> EducationJobs { get; set; }
        public virtual DbSet<GroupComent> GroupComents { get; set; }
        public virtual DbSet<GroupWordComment> GroupWordComments { get; set; }
        public virtual DbSet<GroupWord> GroupWords { get; set; }
        public virtual DbSet<HobbiesInterest> HobbiesInterests { get; set; }
        public virtual DbSet<KeyWord> KeyWords { get; set; }
        public virtual DbSet<KeywordsCount> KeywordsCounts { get; set; }
        public virtual DbSet<LogAddKeyWord> LogAddKeyWords { get; set; }
        public virtual DbSet<LogLabel> LogLabels { get; set; }
        public virtual DbSet<LogVocabulary> LogVocabularies { get; set; }
        public virtual DbSet<MailServer> MailServers { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MySpeciality> MySpecialities { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleFunction> RoleFunctions { get; set; }
        public virtual DbSet<RoleUser> RoleUsers { get; set; }
        public virtual DbSet<SeKeyWord> SeKeyWords { get; set; }
        public virtual DbSet<Sentens> Sentenses { get; set; }
        public virtual DbSet<SkillsAbility> SkillsAbilities { get; set; }
        public virtual DbSet<StopWord> StopWords { get; set; }
        public virtual DbSet<TypeMenu> TypeMenus { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vocabulary> Vocabularies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<Comment>()
                .Property(e => e.GroupCommentId)
                .IsFixedLength();

            modelBuilder.Entity<Contact>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<CountKeyword>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<CountKeyword>()
                .Property(e => e.KeyWordId)
                .IsFixedLength();

            modelBuilder.Entity<CountKeyword>()
                .Property(e => e.GroupCommentId)
                .IsFixedLength();

            modelBuilder.Entity<EducationJob>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<GroupComent>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<GroupComent>()
                .Property(e => e.ProductId)
                .IsFixedLength();

            modelBuilder.Entity<GroupComent>()
                .HasMany(e => e.CountKeywords)
                .WithOptional(e => e.GroupComent)
                .HasForeignKey(e => e.GroupCommentId);

            modelBuilder.Entity<GroupComent>()
                .HasMany(e => e.KeyWords)
                .WithOptional(e => e.GroupComent)
                .HasForeignKey(e => e.GroupCommentId);

            modelBuilder.Entity<GroupComent>()
                .HasMany(e => e.KeywordsCounts)
                .WithOptional(e => e.GroupComent)
                .HasForeignKey(e => e.GroupCommentId);

            modelBuilder.Entity<GroupComent>()
                .HasMany(e => e.LogAddKeyWords)
                .WithOptional(e => e.GroupComent)
                .HasForeignKey(e => e.GroupCommentId);

            modelBuilder.Entity<GroupComent>()
                .HasMany(e => e.LogLabels)
                .WithOptional(e => e.GroupComent)
                .HasForeignKey(e => e.GroupCommentId);

            modelBuilder.Entity<GroupComent>()
                .HasMany(e => e.Vocabularies)
                .WithOptional(e => e.GroupComent)
                .HasForeignKey(e => e.GroupCommentId);

            modelBuilder.Entity<GroupWordComment>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<GroupWordComment>()
                .Property(e => e.GroupWordId)
                .IsFixedLength();

            modelBuilder.Entity<GroupWordComment>()
                .Property(e => e.CommentId)
                .IsFixedLength();

            modelBuilder.Entity<GroupWord>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<GroupWord>()
                .Property(e => e.ProductId)
                .IsFixedLength();

            modelBuilder.Entity<GroupWord>()
                .HasMany(e => e.KeywordsCounts)
                .WithRequired(e => e.GroupWord)
                .HasForeignKey(e => e.GroupKeyWordId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GroupWord>()
                .HasMany(e => e.SeKeyWords)
                .WithOptional(e => e.GroupWord)
                .HasForeignKey(e => e.KeyWordId);

            modelBuilder.Entity<HobbiesInterest>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<KeyWord>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<KeyWord>()
                .Property(e => e.GroupWordId)
                .IsFixedLength();

            modelBuilder.Entity<KeyWord>()
                .Property(e => e.Score)
                .HasPrecision(18, 1);

            modelBuilder.Entity<KeyWord>()
                .Property(e => e.GroupCommentId)
                .IsFixedLength();

            modelBuilder.Entity<KeyWord>()
                .HasMany(e => e.KeywordsCounts)
                .WithRequired(e => e.KeyWord1)
                .HasForeignKey(e => e.KeyWord)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KeywordsCount>()
                .Property(e => e.GroupKeyWordId)
                .IsFixedLength();

            modelBuilder.Entity<KeywordsCount>()
                .Property(e => e.KeyWord)
                .IsFixedLength();

            modelBuilder.Entity<KeywordsCount>()
                .Property(e => e.GroupCommentId)
                .IsFixedLength();

            modelBuilder.Entity<LogAddKeyWord>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<LogAddKeyWord>()
                .Property(e => e.GroupWordId)
                .IsFixedLength();

            modelBuilder.Entity<LogAddKeyWord>()
                .Property(e => e.GroupCommentId)
                .IsFixedLength();

            modelBuilder.Entity<LogLabel>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<LogLabel>()
                .Property(e => e.GroupCommentId)
                .IsFixedLength();

            modelBuilder.Entity<LogVocabulary>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<LogVocabulary>()
                .Property(e => e.CommentId)
                .IsFixedLength();

            modelBuilder.Entity<Menu>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<Menu>()
                .Property(e => e.TypeMenuId)
                .IsFixedLength();

            modelBuilder.Entity<Menu>()
                .Property(e => e.MenuParent)
                .IsFixedLength();

            modelBuilder.Entity<MySpeciality>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<Project>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<Project>()
                .Property(e => e.Code)
                .IsFixedLength();

            modelBuilder.Entity<Role>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<RoleFunction>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<RoleFunction>()
                .Property(e => e.RoleId)
                .IsFixedLength();

            modelBuilder.Entity<RoleFunction>()
                .Property(e => e.FunctionParent)
                .IsFixedLength();

            modelBuilder.Entity<RoleUser>()
                .Property(e => e.Role_Id)
                .IsFixedLength();

            modelBuilder.Entity<SeKeyWord>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<SeKeyWord>()
                .Property(e => e.SeId)
                .IsFixedLength();

            modelBuilder.Entity<SeKeyWord>()
                .Property(e => e.KeyWordId)
                .IsFixedLength();

            modelBuilder.Entity<Sentens>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<Sentens>()
                .Property(e => e.CommentId)
                .IsFixedLength();

            modelBuilder.Entity<Sentens>()
                .HasMany(e => e.SeKeyWords)
                .WithOptional(e => e.Sentens)
                .HasForeignKey(e => e.SeId);

            modelBuilder.Entity<SkillsAbility>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<StopWord>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<TypeMenu>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.RoleId)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Sex)
                .IsFixedLength();

            modelBuilder.Entity<Vocabulary>()
                .Property(e => e.Id)
                .IsFixedLength();

            modelBuilder.Entity<Vocabulary>()
                .Property(e => e.GroupCommentId)
                .IsFixedLength();
        }
    }
}
