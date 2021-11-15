﻿using System;
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

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Topic { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
