
namespace PizzaWebsite.Data.Entities
{
    public class DeliveryOrder: Order
    {
        public string DeliveryArea { get; set; }

        public string DeliveryAddress { get; set; }

        public string PostalCode { get; set; }
    }
}
