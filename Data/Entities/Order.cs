using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
    }

    public class DeliveryOrder : Order
    {

    }

    public class PickupOrder : Order
    {

    }
}
