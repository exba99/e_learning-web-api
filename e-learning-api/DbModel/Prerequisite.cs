using System;
using System.ComponentModel.DataAnnotations;

namespace e_learning_api.DbModel
{
    public class Prerequisite
    {
        public Prerequisite()
        {
        }

        [Key]
        public int PrerequisiteId { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        public string Label { get; set; }

        public int? CourseId { get; set; }

        public Course Course { get; set; }
    }
}
