using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;
using OnlineShop.ViewModels;

namespace OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        public ActionResult Index()
        {
            ShopViewModel model = new ShopViewModel();
            model.Products = db.products.ToList();
            model.Categories = db.categories.ToList();

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Index_admin()
        {
            var moder = db.products.ToList();
            return View(moder);
        }

        // GET: Products
        public ActionResult sreach(string sreach=null)
        {
            IEnumerable<Product> model;
            if(sreach!=null)
            {
                model = db.products.Where(p => p.name.Contains(sreach));
                return View(model);
            }
            else
            return View(db.products.ToList());
        }
        static List<Product> Koszyk = new List<Product>();

        public ActionResult add_to_cart(int? id)
        {
            if (id == null)
                return HttpNotFound();
            var model = db.products.Single(p => p.Id == id);
            if (model == null)
                return HttpNotFound();



            Koszyk.Add(model);

            return RedirectToRoute(new
            {
                controller = "Products",
                action = "Index",

            });


        }

        public ActionResult viewCart()
        {
            return View(Koszyk);
        }


        public ActionResult remove_item_in_cart(int? id)
        {
            if (id == null)
                return HttpNotFound();
            var model = db.products.Single(p => p.Id == id);
            if (model == null)
                return HttpNotFound();

            Product dodaj = (Product)model;



            return RedirectToRoute(new
            {
                controller = "Home",
                action = "Index",

            });




        }


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.products.Where(p => id == p.Id).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            DropDownList();
            return View();
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,description,price,cat_pro")]Product product)
        {
            var model = db.categories.SingleOrDefault(p => p.Id == product.cat_pro.Id);
            if (model != null)
            {
                product.cat_pro = model;
            }
            else
            {
                return HttpNotFound();
            }


            try
            {
                if (ModelState.IsValid)
                {
                    db.products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            DropDownList(product.cat_pro);
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product produkt = db.products.Find(id);
            if (produkt == null)
            {
                return HttpNotFound();
            }
            DropDownList();
            return View(produkt);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (id == null)
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = fileName.Replace(' ','_');
                        product.filePath = Path.Combine(
                            Server.MapPath("~/Content/Images/"), fileName);
                        file.SaveAs(product.filePath);
                    }
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var prduktToUpdate = db.products.Find(id);
            if (TryUpdateModel(prduktToUpdate, "",
               new string[] { "name", "description", "price", "car_pod.id" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            DropDownList(prduktToUpdate.cat_pro);
            return View(prduktToUpdate);
        }

        private void DropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = db.categories.ToList();

            ViewBag.cat_pro = new SelectList(departmentsQuery, "id", "name", selectedDepartment);
        }



        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index_admin");
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