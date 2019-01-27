

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
    public class ExtendUserController: Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
           
            return View(_db.Users.ToList());
        }


        public ActionResult Edit(string id)

        {
           


            var model = _db.Users.Single(p => p.Id == id);
            
            ViewBag.Rola = new SelectList(_db.Roles, "Id", "Name");
           return View(model);
        }


        ////localhost:5528/ExtendUser/Contact

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




        ////localhost:5528/ExtendUser/SendEmail


        public ActionResult SendEmail()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("reklamashoppshopp@gmail.com", "Admin");
                    var receiverEmail = new MailAddress("2e.sienkiewicz@gmail.com", "Receiver");
                    
                    var password = "patrykPp@123";
                    var sub = "test";
                    var body = "Witam dziala a przynajmnije powino";
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };

                    //UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
                    //ApplicationUserManager userManager = new ApplicationUserManager(store);
                    var send_user = _db.Users.Where(p => p.newsletter == true).ToList();
                   

                    var mess = new MailMessage();
                    mess.Body = body;
                    mess.Subject = sub;
                    mess.From = senderEmail;
                    mess.To.Add(receiverEmail);
                    mess.To.Add("sszaring@gmail.com");
                    foreach (var user in send_user)
                    {
                        mess.To.Add(user.Email);
                    }


                    {
                        smtp.Send(mess);
                    }
                    return RedirectToAction("Index", "ExtendUser");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return RedirectToAction("Index", "ExtendUser");
        }






        [HttpPost]
        public ActionResult Edit(ApplicationUser user,string password,string cpassword ,FormCollection form)
        {
            ViewBag.Rola = new SelectList(_db.Roles, "Id", "Name");
            //int id_rola;
            //Int32.TryParse(form["Rola"],out id_rola);

         

            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();


            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser cUser = userManager.FindById(user.Id);
            if (password == cpassword && password!="")
            {
                String userId = user.Id;

                String newPassword = password;
               
                
                String hashedNewPassword = userManager.PasswordHasher.HashPassword(newPassword);
             
                store.SetPasswordHashAsync(cUser, hashedNewPassword);
               


                store.SetPasswordHashAsync(cUser, hashedNewPassword);
                //var x = store.GetPasswordHashAsync(cUser);
                ////user.PasswordHash = x.ToString();
                
                

            }


            cUser.PhoneNumber = user.PhoneNumber;
            cUser.UserName = user.UserName;
            cUser.Email = user.Email;
            var roles = userManager.GetRoles(cUser.Id);


            {
                foreach (var role in roles)
                {
                    userManager.RemoveFromRole(cUser.Id, role);


                }
            }
            cUser.Roles.Add(new IdentityUserRole { RoleId = form["Rola"] });

            store.UpdateAsync(cUser);
            






            return RedirectToAction("Index", "ExtendUser");


          
        }




        public ActionResult Create()
        {
            ViewBag.Rola = new SelectList(_db.Roles, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(string Email, string password, FormCollection form)
        {

            var user = new ApplicationUser { UserName = Email ,Email = Email};
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            
            


            String hashedNewPassword = userManager.PasswordHasher.HashPassword(password);


            var result =  userManager.Create(user, hashedNewPassword);
            store.SetPasswordHashAsync(user, hashedNewPassword);
            user.Roles.Add(new IdentityUserRole { RoleId = form["Rola"] });

            store.UpdateAsync(user);

            
          

            return RedirectToAction("Index", "ExtendUser");








        }



        public ActionResult profil()
        {
            var id = User.Identity.GetUserId();
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser cUser = userManager.FindById(id);


            return View(cUser);
        }


        public ActionResult Edit_profil()
        {
            var id = User.Identity.GetUserId();
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser cUser = userManager.FindById(id);


            return View(cUser);
        }


        [HttpPost]
        public ActionResult Edit_profil(ApplicationUser user, string password, string cpassword, FormCollection form)
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



                store.SetPasswordHashAsync(cUser, hashedNewPassword);
                //var x = store.GetPasswordHashAsync(cUser);
                ////user.PasswordHash = x.ToString();



            }


            cUser.PhoneNumber = user.PhoneNumber;
            cUser.UserName = user.UserName;
            cUser.Email = user.Email;
            store.UpdateAsync(cUser);







            return RedirectToAction("profil", "ExtendUser");



        }







        public ActionResult Delete(string id)
        {
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser cUser= userManager.FindById(id);
            userManager.Delete(cUser);
            
            return RedirectToAction("Index", "ExtendUser");
        }
            
        
        public ActionResult List_discount()
        {
            var model = _db.discount_user.ToList();
            ViewBag.user = _db.Users.ToList();
            return View(model);
        }
           

      

        public ActionResult Delate_discount(int  id)
        {
            var user = _db.discount_user.Where(p => p.Id == id).FirstOrDefault();
            _db.discount_user.Remove(user);
            _db.SaveChanges();

                return RedirectToAction("List_discount", "ExtendUser");
        }



        public ActionResult Creat_discoint()
        {
            ViewBag.User = new SelectList(_db.Users, "Id", "Email");
            


            return View();
        }


        [HttpPost]
        public ActionResult Creat_discoint(discount_for_user discount, FormCollection form)
        {

            var id = form["User"];
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser user = userManager.FindById(id);


            var pom = _db.discount_user.Where(p => p.User.Id == id).FirstOrDefault();
            if(pom!=null)
            _db.discount_user.Remove(pom);

            discount.User = user;
            _db.discount_user.Add(discount);
            _db.SaveChanges();


           
           

            return RedirectToAction("List_discount", "ExtendUser");
        }



    }


}