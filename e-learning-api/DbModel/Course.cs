using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_learning_api.DbModel
{
    public class Course
    {
        public Course()
        {
            Registrations = new HashSet<Registration>();
            Commentaries = new HashSet<Commentary>();
            AudiencesTargeted = new HashSet<AudienceTargeted>();
            Prerequisites = new HashSet<Prerequisite>();
            Purposes = new HashSet<Purpose>();
            PartCourses = new HashSet<PartCourse>();
        }

        [Key]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "*")]
        public string Label { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        public string Image { get; set; }


        [Required(ErrorMessage = "*")]
        [DataType(DataType.DateTime)]
        public DateTime DateAdded { get; set; }

        public string StatusCourse { get; set; }

        public int? NumberOfViews { get; set; }

        public string? OwnerOfCourseId { get; set; }

        public User OwnerOfCourse { get; set; }

        public int? CategoryId { get; set; }

        public Category Category { get; set; }

        public int? LevelCourseId { get; set; }

        public LevelCourse LevelCourse { get; set; }

        public int? LanguageId { get; set; }

        public Language Language { get; set; }

        public string? SpecialisteId { get; set; }

        public User Specialiste { get; set; }

        public ICollection<Registration> Registrations { get; set; }

        public ICollection<Commentary> Commentaries { get; set; }

        public ICollection<AudienceTargeted> AudiencesTargeted { get; set; }

        public ICollection<Prerequisite> Prerequisites { get; set; }

        public ICollection<Purpose> Purposes { get; set; }

        public ICollection<PartCourse> PartCourses { get; set; }

    }
}
