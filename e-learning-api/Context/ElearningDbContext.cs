using System;
using e_learning_api.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace e_learning_api.DbModel
{
    public class ElearningDbContext : IdentityDbContext<ApplicationUser>
    {
        public ElearningDbContext(DbContextOptions<ElearningDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AudienceTargeted>(entity =>
            {
                entity.ToTable("AudienceTargeted");

                entity.HasOne(e => e.Course)
                .WithMany(e => e.AudiencesTargeted)
                .HasForeignKey(e => e.CourseId)
                .HasConstraintName("FK_AudienceTargeted_Course");

            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
            });

            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.ToTable("Chapter");

                entity.HasOne(e => e.PartCourse)
                .WithMany(e => e.Chapters)
                .HasForeignKey(e => e.PartCourseId)
                .HasConstraintName("FK_Chapter_PartCourse");

            });

            modelBuilder.Entity<Commentary>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.CourseId });

                entity.ToTable("Commentary");

                //For User
                entity.HasOne(e => e.User)
                .WithMany(e => e.Commentaries)
                .HasForeignKey(e => e.IdUser)
                .HasConstraintName("FK_Commentary_User");

                //For Course
                entity.HasOne(e => e.Course)
                .WithMany(e => e.Commentaries)
                .HasForeignKey(e => e.CourseId)
                .HasConstraintName("FK_Commentary_Course");

            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                //For Owner Of course
                entity.HasOne(e => e.OwnerOfCourse)
                .WithMany(e => e.CoursesOfOwner)
                .HasForeignKey(e => e.OwnerOfCourseId)
                .HasConstraintName("FK_Course_OwnerOfCourse");

                //For Category
                entity.HasOne(e => e.Category)
                .WithMany(e => e.Courses)
                .HasForeignKey(e => e.CategoryId)
                .HasConstraintName("FK_Course_Category");

                //For Level of Course
                entity.HasOne(e => e.LevelCourse)
                .WithMany(e => e.Courses)
                .HasForeignKey(e => e.LevelCourseId)
                .HasConstraintName("FK_Course_LevelCourse");

                //For Language
                entity.HasOne(e => e.Language)
                .WithMany(e => e.Courses)
                .HasForeignKey(e => e.LanguageId)
                .HasConstraintName("FK_Course_Language");

                //For Specialiste
                entity.HasOne(e => e.Specialiste)
                .WithMany(e => e.CoursesOfSpecialist)
                .HasForeignKey(e => e.SpecialisteId)
                .HasConstraintName("FK_Course_SpecialistUser");

            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("Language");
            });

            modelBuilder.Entity<LevelCourse>(entity =>
            {
                entity.ToTable("LevelCourse");
            });

            modelBuilder.Entity<PartCourse>(entity =>
            {
                entity.ToTable("PartCourse");

                entity.HasOne(e => e.Course)
                .WithMany(e => e.PartCourses)
                .HasForeignKey(e => e.CourseId)
                .HasConstraintName("FK_PartCourse_Course");

            });

            modelBuilder.Entity<Prerequisite>(entity =>
            {
                entity.ToTable("Prerequisite");

                entity.HasOne(e => e.Course)
                .WithMany(e => e.Prerequisites)
                .HasForeignKey(e => e.CourseId)
                .HasConstraintName("FK_Prerequisite_Course");

            });

            modelBuilder.Entity<Purpose>(entity =>
            {
                entity.ToTable("Purpose");

                entity.HasOne(e => e.Course)
                .WithMany(e => e.Purposes)
                .HasForeignKey(e => e.CourseId)
                .HasConstraintName("FK_Purpose_Course");

            });

            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.CourseId });

                entity.ToTable("Registration");

                //For User
                entity.HasOne(e => e.User)
                .WithMany(e => e.Registrations)
                .HasForeignKey(e => e.IdUser)
                .HasConstraintName("FK_Registration_User");

                //For Course
                entity.HasOne(e => e.Course)
                .WithMany(e => e.Registrations)
                .HasForeignKey(e => e.CourseId)
                .HasConstraintName("FK_Registration_Course");

            });

            modelBuilder.Entity<Resources>(entity =>
            {
                entity.ToTable("Resources");

                entity.HasOne(e => e.Chapter)
                .WithMany(e => e.Resources)
                .HasForeignKey(e => e.ChapterId)
                .HasConstraintName("FK_Resources_Chapter");

            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> users { get; set; }
        public DbSet<AudienceTargeted> AudienceTargeteds { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Commentary> Commentaries { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LevelCourse> LevelCourses { get; set; }
        public DbSet<PartCourse> PartCourses { get; set; }
        public DbSet<Prerequisite> Prerequisites { get; set; }
        public DbSet<Purpose> Purposes { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Resources> Resources { get; set; }
    }

}
