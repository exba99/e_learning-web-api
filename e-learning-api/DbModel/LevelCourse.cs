using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_learning_api.DbModel
{
    public class LevelCourse
    {
        public LevelCourse()
        {
            Courses = new HashSet<Course>();
        }

        [Key]
        public int LevelCourseId { get; set; }

        [Required(ErrorMessage = "*"), MaxLength(100, ErrorMessage = "La taille requise est de 100")]
        public string Label { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
