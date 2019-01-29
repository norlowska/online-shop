using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

       

        /// <summary>
        /// GET: Admin/Category
        /// </summary>
        /// <returns>List Category</returns>
        public ActionResult Index()
        {
            var model = db.categories.ToList();
            return View(model);
        }


        /// <summary>
        /// GET: Admin/Category/Create
        /// </summary>
        /// <returns>the new view redirects this form to Create  Category template  </returns>
        public ActionResult Create()
        {
            var items = db.categories.ToList();
            ViewBag.CategoriesList = items;

            return View();
        }

        
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// POST: Categories/Create
        /// </summary>
        /// <param name="category"></param>
        /// <returns>New category</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }


        /// <summary>
        /// GET: Admin/Categories/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Category which be edit</returns>
        public ActionResult Edit(int? id)
        {
            var items = db.categories.ToList();
            ViewBag.CategoriesList = items.Where((cat) => cat.Id != id).ToList();
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

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit([Bind(Include = "Id,name,parent")] Category category)
        {
            if (ModelState.IsValid)
            {
                var parentCategory = db.categories.SingleOrDefault(c => c.Id == category.parent.Id);
                var editedCategory = db.categories.Find(category.Id);
                if (parentCategory != null)
                {
                    editedCategory.parent = parentCategory;
                }
                else
                {
                    editedCategory.parent = null;
                }
                editedCategory.name = category.name;
                //db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }


        /// <summary>
        ///  GET Admin/Categories/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Category which be delete</returns>
        public ActionResult Delete(int? id)
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

        /// <summary>
        /// POST Category  delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns>redirects to aonther action </returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
       
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.categories.Find(id);
            IEnumerable<Product> modele = db.products.Where(p => p.cat_pro.Id == id);
            foreach (var prop in modele)
            {
                db.products.Remove(prop);
            }


            db.categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}