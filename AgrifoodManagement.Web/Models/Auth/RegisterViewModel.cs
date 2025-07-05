using AgrifoodManagement.Util.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace AgrifoodManagement.Web.Models.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "Only letters, spaces, and dashes are allowed.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "Only letters, spaces, and dashes are allowed.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "User type is required.")]
        public UserType? UserType { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\+?[0-9\s-]+$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
    }
}
