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
    [Authorize(Roles = "Admin")]
    public class DiscountController : Controller
    {



        private ApplicationDbContext _db = new ApplicationDbContext();


        //
        /// <summary>
        ///  GET: Admin/Discount
        /// </summary>
        /// <returns>List Discoutn</returns>
        public ActionResult Index()
        {
            var model = _db.discount_user.ToList();
            ViewBag.user = _db.Users.ToList();
            return View(model);
        }








        [Authorize(Roles = "Admin")]

        
        /// <summary>
        ///  Delete Discount by id
        /// </summary>
        /// <returns>Redirect to Action</returns>
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

        /// <summary>
        ///  GET Create new Discount
        /// </summary>
        /// <returns> the new view redirects this form to Create template </returns>
        public ActionResult Create()
        {
            ViewBag.User = new SelectList(_db.Users, "Id", "Email");



            return View();
        }

        /// <summary>
        ///  httpPost Create new Discount
        /// </summary>
        /// <param name="discount"></param>
        /// <param name="form"></param>
        /// <returns>Redirect to Action  </returns>
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