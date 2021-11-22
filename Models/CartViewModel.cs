﻿using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal Total { get; set; }
    }
}
