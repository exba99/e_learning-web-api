using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_learning_api.DbModel
{
    public class Chapter
    {
        public Chapter()
        {
            Resources = new HashSet<Resources>();
        }

        [Key]
        public int ChapterId { get; set; }

        [Required(ErrorMessage = "*")]
        public string Title { get; set; }

        public int Order { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        public string VideoFile { get; set; }

        public int? PartCourseId { get; set; }

        public PartCourse PartCourse { get; set; }

        public ICollection<Resources> Resources { get; set; }
    }
}
