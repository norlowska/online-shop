using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Models
{
    [Bind(Exclude = "OrderId")]
    public partial class Order
    {
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }
        [ScaffoldColumn(false)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Imię jest wymagane")]
        [DisplayName("Imię")]
        [StringLength(160)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [DisplayName("Nazwisko")]
        [StringLength(160)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Adres jest wymagany")]
        [DisplayName("Adres")]
        [StringLength(160)]
        public string Address { get; set; }
        [Required(ErrorMessage = "Miasto jest wymagane")]
        [DisplayName("Miasto")]
        [StringLength(160)]
        public string City { get; set; }
        [Required(ErrorMessage = "Województwo jest wymagane")]
        [DisplayName("Województwo")]
        [StringLength(160)]
        public string State { get; set; }
        [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
        [DisplayName("Kod pocztowy")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}",
            ErrorMessage = "Kod pocztowy jest nieprawidłowy.")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Kraj jest wymagany")]
        [DisplayName("Kraj")]
        [StringLength(160)]
        public string Country { get; set; }
        [Required(ErrorMessage = "Telefon jest wymagany")]
        [DisplayName("Telefon")]
        [StringLength(160)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "E-mail jest wymagany")]
        [DisplayName("E-mail")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "E-mail jest nieprawidłowy.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [ScaffoldColumn(false)]
        public decimal Total { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }


}