using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Entities
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        private const int MIN_LENGTH = 2;
        private const int MAX_LENGTH = 50;

        [Required]
        [MinLength(MIN_LENGTH)]
        [MaxLength(MAX_LENGTH)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(MIN_LENGTH)]
        [MaxLength(MAX_LENGTH)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(7)]
        public string PostalCode { get; set; }

        [Required]
        [MaxLength(30)]
        public string Email { get; set; }

        [Required]
        public string Topic { get; set; }

        public string Phone { get; set; }

        [Required]
        [MaxLength(300)]
        public string Message { get; set; }
    }
}
