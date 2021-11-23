using System;
using System.ComponentModel.DataAnnotations;

namespace e_learning_api.DbModel
{
    public class Resources
    {
        public Resources()
        {
        }

        [Key]
        public int ResourcesId { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        public string ResourcesFile { get; set; }

        public int? ChapterId { get; set; }

        public Chapter Chapter { get; set; }
    }
}
