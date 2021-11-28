using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        // TODO: create user table
        public int UserId { get; set; }

        public int ProductId { get; set; }
    
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
