using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Models
{
    public class OrderViewModel
    {
        //TODO
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public UserViewModel User { get; set; }
    }
}
