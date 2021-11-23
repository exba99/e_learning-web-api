using System;
using System.ComponentModel.DataAnnotations;

namespace e_learning_api.DbModel
{
    public class Registration
    {
        public Registration()
        {
        }

        public string IdUser { get; set; }

        public int CourseId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateRegistration { get; set; }

        public User User { get; set; }

        public Course Course { get; set; }
    }
}
