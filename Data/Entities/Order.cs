using System;
using System.Collections.Generic;

namespace PizzaWebsite.Data.Entities
{
    public enum PaymentType
    {
        Cash,
        Credit
    }

    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public PaymentType PaymentType { get; set; }

        public string OrderEmail { get; set; }

        public DateTime OrderTime { get; set; }
    }

    public class DeliveryOrder : Order
    {
        public string DeliveryArea { get; set; }
        
        public string DeliveryAddress { get; set; }

        public string PostalCode { get; set; }
    }
}
