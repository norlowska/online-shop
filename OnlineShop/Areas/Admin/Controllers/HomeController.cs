using OnlineShop.Areas.Admin.Models;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Home
        public ActionResult Index()
        {            
            return View();
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult Settings()
        {
            var adminSettings = db.adminSettings.ToList().LastOrDefault();
            return PartialView(adminSettings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings([Bind(Include = "Id,ContactAddress")]GeneralSettings adminSettings)
        {
            if (ModelState.IsValid)
            {
                db.adminSettings.Add(adminSettings);
                db.SaveChanges();
            }
            return PartialView(adminSettings);
        }
    }
}