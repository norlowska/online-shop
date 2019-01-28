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
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Admin/User
        public ActionResult Index()
        {
            var model = db.Users.ToList();
            return View(model);
        }

        // GET: Admin/User/Create
        public ActionResult Create()
        {
            ViewBag.Rola = new SelectList(db.Roles, "Id", "Name");
            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        public ActionResult Create(string Email, string password, FormCollection form)
        {
            var user = new ApplicationUser { UserName = Email, Email = Email };
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);

            String hashedNewPassword = userManager.PasswordHasher.HashPassword(password);

            var result = userManager.Create(user, hashedNewPassword);
            store.SetPasswordHashAsync(user, hashedNewPassword);
            user.Roles.Add(new IdentityUserRole { RoleId = form["Rola"] });

            store.UpdateAsync(user);

            return RedirectToAction("Index", "ExtendUser");
        }

        // GET: Admin/User/Edit/{id}
        public ActionResult Edit(string id)
        {
            var model = db.Users.Single(p => p.Id == id);
            ViewBag.Rola = new SelectList(db.Roles, "Id", "Name");
            return View(model);
        }

        // POST: Admin/User/Edit
        [HttpPost]
        public ActionResult Edit(ApplicationUser user, string password, string cpassword, FormCollection form)
        {
            ViewBag.Rola = new SelectList(db.Roles, "Id", "Name");
            //int id_rola;
            //Int32.TryParse(form["Rola"],out id_rola);
           
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser cUser = userManager.FindById(user.Id);
            if (password == cpassword && password != "")
            {
                String userId = user.Id;
                String newPassword = password;
                String hashedNewPassword = userManager.PasswordHasher.HashPassword(newPassword);
                store.SetPasswordHashAsync(cUser, hashedNewPassword);
                //store.SetPasswordHashAsync(cUser, hashedNewPassword);
                //var x = store.GetPasswordHashAsync(cUser);
                ////user.PasswordHash = x.ToString();
            }

            cUser.PhoneNumber = user.PhoneNumber;
            cUser.UserName = user.UserName;
            cUser.Email = user.Email;
            var roles = userManager.GetRoles(cUser.Id);
            foreach (var role in roles)
            {
                userManager.RemoveFromRole(cUser.Id, role);
            }
            cUser.Roles.Add(new IdentityUserRole { RoleId = form["Rola"] });
            store.UpdateAsync(cUser);

            return RedirectToAction("Index", "ExtendUser");
        }

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
                    var send_user = db.Users.Where(p => p.newsletter == true).ToList();


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
                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return RedirectToAction("Index", "User");
        }
    }
}