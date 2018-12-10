
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExtendUserController : Controller
    {
        protected ApplicationDbContext _db = new ApplicationDbContext();
    
        public ActionResult Index()
        {
           
            return View(_db.Users.ToList());
        }


        public ActionResult Edit(string id)
        {
            var model = _db.Users.Single(p => p.Id == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ApplicationUser user,string password,string cpassword)
        {


            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();
            if (password == cpassword && password!="")
            {
                String userId = user.Id;

                String newPassword = password;
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
                ApplicationUserManager userManager = new ApplicationUserManager(store);
                ApplicationUser cUser = userManager.FindById(user.Id);
                cUser.PhoneNumber = user.PhoneNumber;
                cUser.UserName = user.UserName;
                cUser.Email = user.Email;
                
                String hashedNewPassword = userManager.PasswordHasher.HashPassword(newPassword);
             
                store.SetPasswordHashAsync(cUser, hashedNewPassword);
               


                store.SetPasswordHashAsync(cUser, hashedNewPassword);
                //var x = store.GetPasswordHashAsync(cUser);
                ////user.PasswordHash = x.ToString();
                store.UpdateAsync(cUser);
                

            }
            
               

            


           

            

            



            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(RegisterViewModel model,string password)
        {

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            var result =  userManager.CreateAsync(user, password);
            if (result.IsCompleted)
            {
               

                

                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        public ActionResult Delete(string id)
        {
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser cUser= userManager.FindById(id);
            userManager.Delete(cUser);
            return RedirectToAction("Index", "Home");
        }
            
            
           




    }


}