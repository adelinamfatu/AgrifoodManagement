using AgrifoodManagement.Util.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Domain.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Avatar { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public UserType UserType { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
