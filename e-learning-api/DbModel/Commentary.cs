using System;
using System.ComponentModel.DataAnnotations;

namespace e_learning_api.DbModel
{
    public class Commentary
    {
        public string IdUser { get; set; }

        public int CourseId { get; set; }

        [DataType(DataType.Text)]
        public string Comment { get; set; }

        public double Note { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DatePublished { get; set; }

        public User User { get; set; }

        public Course Course { get; set; }
    }
}
