using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_learning_api.DbModel
{
    public class Category
    {
        public Category()
        {
            Courses = new HashSet<Course>();
        }

        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "*")]
        public string Label { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
