using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Entities
{
    public class CartItem
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int ProductId { get; set; }

        public int PortionId { get; set; }

        public ProductPortion ProductPortion { get; set; }

        public int Quantity { get; set; }
    }
}
