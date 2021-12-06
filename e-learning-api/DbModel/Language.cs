using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_learning_api.DbModel
{
    public class Language
    {
        public Language()
        {
            Courses = new HashSet<Course>();
        }

        [Key]
        public int LanguageId { get; set; }

        [Required(ErrorMessage = "*")]
        public string Label { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
