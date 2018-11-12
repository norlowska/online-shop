using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {

        List<Product> products;
        public ProductController()
        {
            Category child1 = new Category { Id = 101, name = "MYSZKI", Children = null };
            Category child2 = new Category { Id = 102, name = "KLAWIATURY", Children = null };
            List<Category> children = new List<Category>();
            children.Add(child1);
            children.Add(child2);

            Category cat = new Category { Id = 1, name = "AKCESORIA", Children = children };
            products = new List<Product>
        {
            new Product { Id=1,name="Myszka1",price=20,description="to jest myszka1", Category=(cat.Children).ElementAt(0) },
             new Product { Id=2,name="Myszka2",price=20,description="to jest myszka2", Category=(cat.Children).ElementAt(0) },
              new Product { Id=3,name="Myszka3",price=20,description="to jest myszka2", Category=(cat.Children).ElementAt(0) },
               new Product { Id=4,name="Klawiatura1",price=20,description="to jest klawiatura", Category=(cat.Children).ElementAt(1) },
        };
        }
        // GET: Product
        public ActionResult Index()
        {
            var model = products;
            return View(products);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            var model = products.FirstOrDefault(p => p.Id == id);
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {

            // TODO: Add update logic here
            var model = products.Single(p => p.Id == id);


            return View(model);

        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}