

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

       
    }
}