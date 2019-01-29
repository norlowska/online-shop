using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineShop.Models;
using OnlineShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public ActionResult Index(int str=0)
        {

            var id = User.Identity.GetUserId();
            float zniszka = 1;
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(storeDB);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser cUser = userManager.FindById(id);
            var discount = storeDB.discount_user.Where(p => p.User.Id == cUser.Id).FirstOrDefault();
            ViewBag.str = str;

            if (discount == null )
            {
                ViewBag.discount = 0;
            }
            else
            {
                ViewBag.discount =1;
            }

            if(str==0)
            {
                 zniszka = (float)discount.percent / 100;
            }
            if(str==1)
            {
                zniszka = 1;
            }


            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(zniszka)
            };

            return View(viewModel);
        }
        // GET: /store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            var addedProduct = storeDB.products.Single(c => c.Id == id);
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedProduct);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(HttpContext);
            string productName = storeDB.Carts.Single(itm => itm.RecordId == id).Product.name;
            int count = cart.RemoveFromCart(id);

            return RedirectToAction("Index");
        }

        //GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(HttpContext);
            ViewData["CartCount"] = cart.GetCount();

            return PartialView("CartSummary");
        }
    }
}