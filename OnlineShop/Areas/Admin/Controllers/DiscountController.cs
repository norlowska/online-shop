using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineShop.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class DiscountController : Controller
    {




        private ApplicationDbContext _db = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        // GET: Admin/Discount
        public ActionResult Index()
        {
            var model = _db.discount_user.ToList();
            ViewBag.user = _db.Users.ToList();
            return View(model);
        }

      


     



        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _db.discount_user.Where(p => p.Id == id).FirstOrDefault();
            _db.discount_user.Remove(user);
            _db.SaveChanges();

            return RedirectToAction("Index", "Discount");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.User = new SelectList(_db.Users, "Id", "Email");



            return View();
        }


        [HttpPost]
        public ActionResult Create(discount_for_user discount, FormCollection form)
        {

            var id = form["User"];
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser user = userManager.FindById(id);


            var pom = _db.discount_user.Where(p => p.User.Id == id).FirstOrDefault();
            if (pom != null)
                _db.discount_user.Remove(pom);

            discount.User = user;
            _db.discount_user.Add(discount);
            _db.SaveChanges();





            return RedirectToAction("Index", "Discount");
        }



    }
}