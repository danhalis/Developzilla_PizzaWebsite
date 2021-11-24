using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        // TODO: Set up M-1 relationship with ProductCategory
        //[Required]
        //[ForeignKey("CategoryId")]
        //public int CategoryId { get; set; }

        //public ProductCategory Category { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
