using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_learning_api.DbModel
{
    public class User
    {

        public User()
        {
            Registrations = new HashSet<Registration>();
            Commentaries = new HashSet<Commentary>();
            CoursesOfOwner = new HashSet<Course>();
            CoursesOfSpecialist = new HashSet<Course>();
        }

        [Key]
        public string IdUser { get; set; }

        [Required(ErrorMessage = "*"), MaxLength(100, ErrorMessage = "La taille requise est de 100")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*"), MaxLength(100, ErrorMessage = "La taille requise est de 100")]
        public string EmailContact { get; set; }

        [Required(ErrorMessage = "*")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*"), MaxLength(100, ErrorMessage = "La taille requise est de 100")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*"), MaxLength(100, ErrorMessage = "La taille requise est de 100")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        public string Avatar { get; set; }

        [Required(ErrorMessage = "*"), MaxLength(12, ErrorMessage = "La taille requise est de 20")]
        public string PhoneNumber { get; set; }

        public string? Speciality { get; set; }

        public string? Biography { get; set; }

        public string? StatusUser { get; set; }

        public string? IdRole { get; set; }

        public ICollection<Registration> Registrations { get; set; }

        public ICollection<Commentary> Commentaries { get; set; }

        public ICollection<Course> CoursesOfOwner { get; set; }

        public ICollection<Course> CoursesOfSpecialist { get; set; }
    }
}
