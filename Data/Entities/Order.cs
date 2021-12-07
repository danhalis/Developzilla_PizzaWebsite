using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public Cart Cart { get; set; }

        public PaymentType PaymentType { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public DateTime OrderTime { get; set; }
    }

    public enum PaymentType
    {
        Cash,
        Credit
    }

    public class DeliveryOrder : Order
    {
        public string DeliveryArea { get; set; }

        public string DeliveryAddress { get; set; }

        public string PostalCode { get; set; }
    }
}
