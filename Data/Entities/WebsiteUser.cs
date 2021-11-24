using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Entities
{
    public class WebsiteUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        public byte[] ProfilePicture { get; set; }

        public string DeliveryAddress { get; set; }

   //  TODO  
   //   public List<OrderViewModel> Orders { get; set; } 

    }
}
