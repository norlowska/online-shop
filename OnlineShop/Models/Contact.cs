using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class Contact : IEntit
    {
        [Required(ErrorMessage = "First name is required")]
        public String FirstName { get; set; }
        [Required]
        public String Subject { get; set; }
        [Required]
        [EmailAddress]
        public String Email { get; set; }
        [Required]
        public String Message { get; set; }
    }
}