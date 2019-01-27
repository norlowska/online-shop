

using Microsoft.AspNet.Identity.EntityFramework;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExtendUserController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(_db.Users.ToList());
        }
        
        //// ExtendUser/Contact

        //public async  Task<ActionResult> Contact()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
        //        var message = new MailMessage();
        //        message.To.Add(new MailAddress("2e.sienkiewcz@gmail.com"));  // replace with valid value 
        //        message.From = new MailAddress("Sklep_Sklep@outlook.com");  // replace with valid value
        //        message.Subject = "Your email subject";
        //        message.Body = string.Format(body,"Admin", "Sklep_Sklep@outlook.com","Witam");
        //        message.IsBodyHtml = true;

        //        using (var smtp = new SmtpClient())
        //        {
        //            var credential = new NetworkCredential
        //            {
        //                UserName = "Sklep_Sklep@outlook.com",  // replace with valid value
        //                Password = "patrykPp@123"  // replace with valid value
        //            };
        //            smtp.Credentials = credential;
        //            smtp.Host = "smtp-mail.outlook.com";
        //            smtp.Port = 587;
        //            smtp.EnableSsl = true;
        //            await smtp.SendMailAsync(message);
        //            return RedirectToAction("Index", "ExtendUser");
        //        }
        //    }
        //    return RedirectToAction("Index", "ExtendUser");
        //}

            
        public ActionResult Profile()
        {
            var id = User.Identity.GetUserId();
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser cUser = userManager.FindById(id);

            return View(cUser);
        }


        public ActionResult EditProfile()
        {
            var id = User.Identity.GetUserId();
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser cUser = userManager.FindById(id);

            return View(cUser);
        }


        [HttpPost]
        public ActionResult EditProfile(ApplicationUser user, string password, string cpassword, FormCollection form)
        {
            //int id_rola;
            //Int32.TryParse(form["Rola"],out id_rola);         
            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();

            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser cUser = userManager.FindById(user.Id);
            if (password == cpassword && password != "")
            {
                String userId = user.Id;
                String newPassword = password;
                String hashedNewPassword = userManager.PasswordHasher.HashPassword(newPassword);

                store.SetPasswordHashAsync(cUser, hashedNewPassword);
                //  store.SetPasswordHashAsync(cUser, hashedNewPassword);
                //var x = store.GetPasswordHashAsync(cUser);
                ////user.PasswordHash = x.ToString();

            }

            cUser.PhoneNumber = user.PhoneNumber;
            cUser.UserName = user.UserName;
            cUser.Email = user.Email;
            store.UpdateAsync(cUser);

            return RedirectToAction("Profile", "ExtendUser");
        }

        public ActionResult Delete(string id)
        {
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser cUser = userManager.FindById(id);
            userManager.Delete(cUser);

            return RedirectToAction("Index", "ExtendUser");
        }


        public ActionResult ListDiscounts()
        {
            var model = _db.discount_user.ToList();
            ViewBag.user = _db.Users.ToList();
            return View(model);
        }

        public ActionResult DeleteDiscount(int id)
        {
            var user = _db.discount_user.Where(p => p.Id == id).FirstOrDefault();
            _db.discount_user.Remove(user);
            _db.SaveChanges();

            return RedirectToAction("ListDiscounts", "ExtendUser");
        }

        public ActionResult CreateDiscount()
        {
            ViewBag.User = new SelectList(_db.Users, "Id", "Email");
            return View();
        }

        [HttpPost]
        public ActionResult CreateDiscount(discount_for_user discount, FormCollection form)
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

            return RedirectToAction("ListDiscounts", "ExtendUser");
        }
    }
}