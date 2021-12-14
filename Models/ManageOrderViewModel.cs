using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Models
{
    public class ManageOrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public Dictionary<int, decimal> TotalForeachOrder { get; set; }
        public decimal Total { get; set; }
    }
}
