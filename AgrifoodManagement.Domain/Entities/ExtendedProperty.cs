using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Domain.Entities
{
    [Table("ExtendedProperties")]
    public class ExtendedProperty
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string EntityType { get; set; }

        [Required]
        public Guid EntityId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
