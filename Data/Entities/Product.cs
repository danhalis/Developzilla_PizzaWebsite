using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Data.Entities
{
    public class Product
    {
        public Product()
        {
            SizePrices = new List<ProductSizePrice>();
        }
        
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public ProductCategory Category { get; set; }

        [Required]
        public string ImageName { get; set; }

        public List<ProductSizePrice> SizePrices { get; set; }
    }

    public class DeserializedProduct
    {
        public DeserializedProduct()
        {
            Sizes = new List<string>();
            Prices = new List<decimal>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string ImageName { get; set; }

        public List<string> Sizes { get; set; }

        public List<decimal> Prices { get; set; }
    }
}
