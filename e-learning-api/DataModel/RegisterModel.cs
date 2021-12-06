using System;
namespace e_learning_api.DataModel
{
    public class RegisterModel
    {
        public string Email { get; set; }

        public string EmailContact { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Avatar { get; set; }

        public string PhoneNumber { get; set; }

        public string? Speciality { get; set; }

        public string? Biography { get; set; }

        public string RoleId { get; set; }
    }
}
