using System;
using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Data.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int CartId { get; set; }

        public Cart Cart { get; set; }

        public PaymentType PaymentType { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public DateTime OrderTime { get; set; }
    }

    public enum Status
    {
        Ordered, //When front end or user makes an order.
        Preparing, //When cook is cooking items in an order.
        Ready, //When cook is done cooking items and its ready for pickup/delivery
        Pending, //When either currently delivering or currently waiting for pickup
        Completed //When order has been delivered or picked up.
    }

    public enum PaymentType
    {
        Cash,
        Credit
    }

    public enum ReceptionMethod
    {
        Pickup,
        Delivery
    }
}
