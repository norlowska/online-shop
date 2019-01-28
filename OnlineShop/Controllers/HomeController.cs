using OnlineShop.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var model = _db.products.ToList();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        [ChildActionOnly]
        public ActionResult Visits()
        {
            var counter = _db.counters.First();
            return PartialView(counter);
        }

        //GET: Home/Contact
        public ActionResult Contact()
        {
            return View();
        }

        //POST: Home/Contact
        [HttpPost]
        public ActionResult Contact(Contact c)
        {
            var addressSettings = _db.adminSettings.ToList().LastOrDefault();
            if (addressSettings != null)
            {
                var toAddress = addressSettings.ContactAddress;
                if (ModelState.IsValid)
                {
                    var fromAddress = c.Email.ToString();
                    var subject = c.Subject;
                    var message = new StringBuilder();
                    message.Append("Name: " + c.FirstName + "<br />");
                    message.Append("Email: " + c.Email + "<br />");
                    message.Append(c.Message);
                    var tEmail = new Thread(() =>
                        SendEmail(toAddress, fromAddress, subject, message.ToString()));
                    tEmail.Start();
                }
                return RedirectToAction("ContactSuccess");
            }
            return RedirectToAction("ContactFail");            
        }

        public void SendEmail(string toAddress, string fromAddress,
                      string subject, string message)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    const string email = "techtronics.contact@gmail.com";
                    const string password = "asp_MVC19";

                    var loginInfo = new System.Net.NetworkCredential(email, password);


                    mail.From = new MailAddress(fromAddress);
                    mail.To.Add(new MailAddress(toAddress));
                    mail.Subject = subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;

                    try
                    {
                        using (var smtpClient = new SmtpClient(
                                                         "smtp.gmail.com", 587))
                        {
                            smtpClient.EnableSsl = true;
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = loginInfo;
                            smtpClient.Send(mail);
                        }

                    }

                    finally
                    {
                        //dispose the client
                        mail.Dispose();
                    }

                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (SmtpFailedRecipientException t in ex.InnerExceptions)
                {
                    var status = t.StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Response.Write("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        //resend
                        //smtpClient.Send(message);
                    }
                    else
                    {
                        Response.Write("Failed to deliver message to " + t.FailedRecipient);
                    }
                }
            }
            catch (SmtpException Se)
            {
                // handle exception here
                Response.Write(Se.ToString());
            }

            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }

        public ActionResult ContactSuccess()
        {
            return View();
        }

        public ActionResult ContactFail()
        {
            return View();
        }

        public ActionResult Buy(int id)
        {
            var model = _db.products.Where(p => p.Id == id);


            return View(model);
        }

        
    }
}