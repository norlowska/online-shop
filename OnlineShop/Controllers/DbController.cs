using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class DbController : Controller
    {
        /// <summary>
        /// Dbcontex
        /// </summary>
        protected ApplicationDbContext _db = new ApplicationDbContext();
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}