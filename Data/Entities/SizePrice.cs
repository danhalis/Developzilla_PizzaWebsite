

using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Data.Entities
{
    public class ProductSizePrice
    {
        public int Id { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        public decimal Price { get; set; }

        public Product Product { get; set; }

        public int ProductId { get; set; }
    }
}
