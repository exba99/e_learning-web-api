using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_learning_api.DbModel
{
    public class PartCourse
    {
        public PartCourse()
        {
            Chapters = new HashSet<Chapter>();
        }

        [Key]
        public int PartCourseId { get; set; }

        [Required(ErrorMessage = "*")]
        public string Title { get; set; }

        public int Order { get; set; }

        public int? CourseId { get; set; }

        public Course Course { get; set; }

        public ICollection<Chapter> Chapters { get; set; }

    }
}
