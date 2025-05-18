using System.ComponentModel.DataAnnotations;

namespace AgrifoodManagement.Web.Models.Settings
{
    public class UpdateUserViewModel
    {
        public Guid UserId { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; } = "";

        public string? Avatar { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; } = "";

        [Display(Name = "Last name")]
        public string LastName { get; set; } = "";

        [Phone]
        [Display(Name = "Phone number")]
        [RegularExpression(
            @"^(\+?\d{1,3}[-\s]?)?
            (\(?\d+\)?[-\s]?)*
            (\s?(ext|x|extension)\s?\d{1,5})?$",
          ErrorMessage = "Must be a valid phone (e.g. +44 20 7123 4567 ext 1234)")]
        public string PhoneNumber { get; set; } = "";

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "City")]
        public string? City { get; set; }

        [Display(Name = "Street")]
        public string? Street { get; set; }

        [Display(Name = "Street Number")]
        public string? Number { get; set; }

        [Display(Name = "Country")]
        public string? Country { get; set; }
    }
}
