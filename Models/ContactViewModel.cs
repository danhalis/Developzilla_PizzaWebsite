using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaWebsite.Models
{
    /*
    * Course: 		Web Programming 3
    * Assessment: 	Assignment 3
    * Created by: 	HIEU DAO LE DUC
    * Date: 		04 NOVEMBER 2021
    * Class Name: 	ContactViewModel.cs
    * Description:  Represents a view model with Contact information and reCAPTCHA response.
    */
    public class ContactViewModel
    {
        private static List<string> _topics = new List<string>()
        {
            "My order",
            "Feedback",
            "Product questions",
            "Customer service and feedback",
            "Technical questions, specifications, geometry, sizing and historical information",
            "Warranty",
            "Registration",
            "Catalogue requests",
            "Owner's manuals",
            "Media enquiries",
            "Sponsorship and donations"
        };

        private const int MIN_LENGTH = 2;
        private const int NAME_MAX_LENGTH = 50;

        [Required]
        [RegularExpression("^[^0-9]+$", ErrorMessage = "First name cannot contain numbers.")]
        [MinLength(MIN_LENGTH, ErrorMessage = "First name must have at least 2 characters.")]
        [MaxLength(NAME_MAX_LENGTH, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[^0-9]+$", ErrorMessage = "Last name cannot contain numbers.")]
        [MinLength(MIN_LENGTH, ErrorMessage = "Last name must have at least 2 characters.")]
        [MaxLength(NAME_MAX_LENGTH, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^([A-Y]\d[A-Z])\ {0,1}(\d[A-Z]\d)$", ErrorMessage = "Please enter a valid postal code.")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Please enter a valid email address.")]
        [MinLength(MIN_LENGTH, ErrorMessage = "Email address must have at least 2 characters.")]
        [MaxLength(30, ErrorMessage = "Email address cannot exceed 30 characters.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Retrieves a list of pre-defined topics.
        /// </summary>
        public static List<string> Topics
        {
            get { return _topics; }
        }

        [Required]
        public string Topic { get; set; }

        [Required]
        public string Message { get; set; }

        public string ReCaptchaResponse { get; set; }
    }
}
