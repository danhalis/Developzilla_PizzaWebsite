using PizzaWebsite.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Models
{
    public class CheckoutViewModel
    {
        [Required]
        [RegularExpression("^[^0-9]+$", ErrorMessage = "First name cannot contain numbers.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[^0-9]+$", ErrorMessage = "Last name cannot contain numbers.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@".*@.*\.\w{2,}", ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class DeliveryCheckoutViewModel : CheckoutViewModel
    {
        // Assisted by https://stackoverflow.com/questions/15774555/efficient-regex-for-canadian-postal-code-function
        [RegularExpression(@"^[ABCEGHJ-NPRSTVXY]\d[ABCEGHJ-NPRSTV-Z][ -]?\d[ABCEGHJ-NPRSTV-Z]\d$", ErrorMessage = "Please enter a valid Canadian Postal Code.")]
        [DataType(DataType.PostalCode)]
        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string DeliveryArea { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        private static List<string> _deliveryAreas = new List<string>()
        {
            "Sainte-Anne-De-Bellevue",
            "Baie-D'Urfé",
            "Senneville",
            "Kirkland",
            "Dollard-Des-Ormeaux",
            "Beaconsfield",
            "Pierrefonds and Roxboro",
            "L'Île-Bizard–Sainte-Geneviève",
            "Pointe-Claire",
            "Dorval"
        };

        /// <summary>
        /// Retrieves a list of pre-defined delivery areas.
        /// </summary>
        public static List<string> DeliveryAreas
        {
            get { return _deliveryAreas; }
        }
    }
}
