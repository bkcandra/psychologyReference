namespace ReferencingSystem.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RsContext : DbContext
    {
        public RsContext()
            : base("name=RsContext")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseLevel> CourseLevel { get; set; }
        public virtual DbSet<EmailSetting> EmailSetting { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }
        public virtual DbSet<Pages> Pages { get; set; }
        public virtual DbSet<PaymentRecords> PaymentRecords { get; set; }
        public virtual DbSet<SubscriptionPlan> SubscriptionPlan { get; set; }
        public virtual DbSet<University> University { get; set; }
        public virtual DbSet<UserProfiles> UserProfiles { get; set; }
        public virtual DbSet<UniversityAdmin> UniversityAdmin { get; set; }
        public virtual DbSet<UserSubscription> UserSubscription { get; set; }

        public virtual DbSet<Reference> Reference { get; set; }
        public virtual DbSet<ReferenceAnswer> ReferenceAnswer { get; set; }
        public virtual DbSet<ReferenceFiles> ReferenceFiles { get; set; }
        public virtual DbSet<ReferenceShareRecord> ReferenceShareRecord { get; set; }
        public virtual DbSet<ReferenceCourse> ReferenceCourse { get; set; }
        public virtual DbSet<DownloadRecords> DownloadRecords { get; set; }
        public virtual DbSet<Navigation> Navigation { get; set; }
        public virtual DbSet<WebAssets> WebAssets { get; set; }
        public virtual DbSet<WebConfig> WebConfig { get; set; }

        public virtual DbSet<RefForm> RefForm { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionOption> QuestionOption { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
              .HasOptional(e => e.UserProfiles)
              .WithRequired(e => e.AspNetUsers)
              .WillCascadeOnDelete();

            modelBuilder.Entity<AspNetUsers>()
            .HasOptional(e => e.UserSubscription)
            .WithRequired(e => e.AspNetUsers)
            .WillCascadeOnDelete();

            modelBuilder.Entity<UserProfiles>()
                .HasOptional(e => e.UniversityAdmin)
                .WithRequired(e => e.UserProfiles)
                .WillCascadeOnDelete();

        }
    }
}
