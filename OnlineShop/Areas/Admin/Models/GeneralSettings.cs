using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class GeneralSettings :IEntit
    {
        [Required, EmailAddress]
        public String ContactAddress { get; set; }
    }
}