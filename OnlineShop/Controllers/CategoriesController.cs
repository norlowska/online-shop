using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class CategoriesController : Controller
    {


       

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        /// <summary>
        /// Get Categories in database
        /// </summary>
        /// 
        /// <returns>List Categories</returns>

        public ActionResult Index()
        {
            return View(db.categories.ToList());
        }

        // GET: Categories/Details/5
        /// <summary>
        /// Get one Categorie by id  in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Categorie</returns>

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        } 



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
