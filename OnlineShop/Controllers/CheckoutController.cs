using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{

    [Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();



        /// <summary>
        ///  GET: Checkout
        /// </summary>
        /// <returns>List Checkout</returns>
        public ActionResult AddressAndPayment()
        {
            var order = new Order();
            TryUpdateModel(order);
            try
            {
                var id = User.Identity.GetUserId();
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(storeDB);
                ApplicationUserManager userManager = new ApplicationUserManager(store);
                ApplicationUser user = userManager.FindById(id);
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;
                order.FirstName = user.imie;
                order.LastName = user.Nazwisko;
                order.Address = user.Adres;
                order.City = user.miasto;
                order.State = user.Województwo;
                order.PostalCode = user.kod_pocztowy;
                order.Country = user.Kraj;
                order.Phone = user.PhoneNumber;
                order.Email = user.Email;

            }
            catch
            {
                return View(order);
            }
            return View(order);
        }


        /// <summary>
        /// AddresandPAyment  
        /// </summary>
        /// <param name="order"></param>
        /// <returns>  Redirect To Action</returns>
        [HttpPost]
        public ActionResult AddressAndPayment(Order order)
        {
            TryUpdateModel(order);
            try
            {
                order.orderState = OrderState.New;
                storeDB.Orders.Add(order);
                storeDB.SaveChanges();

                //TODO: Email do usera

                var cart = ShoppingCart.GetCart(HttpContext);
                cart.CreateOrder(order);
                return RedirectToAction("Complete", new { id = order.OrderId });
            }
            catch
            {
                return View(order);
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Complete(int id)
        {
            bool isValid = storeDB.Orders.Any(o => o.OrderId == id && o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}