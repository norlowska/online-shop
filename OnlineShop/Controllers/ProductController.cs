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

        /// <summary>
        /// 
        /// </summary>
       
        List<Product> products = new List<Product>
        {
            new Product { Id=1,name="Myszka1",price=20,description="to jest myszka1" },
             new Product { Id=2,name="Myszka2",price=40,description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse eget pellentesque lectus. Praesent ac purus vel sapien sagittis elementum. Integer eget bibendum libero. Donec hendrerit id lectus ut bibendum. Aliquam ullamcorper quam tortor, nec interdum arcu vulputate quis. " },
              new Product { Id=3,name="Myszka3",price=20,description=@"<i>Lorem ipsum dolor sit amet, consectetur adipiscing elit.<i>" },
               new Product { Id=4,name="Myszka4",price=20,description="to jest myszka3" },
        };

     
        // GET: Product
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            
            return View();
        }





        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            var model = products.FirstOrDefault(p => p.Id == id);
            return View(model);
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
            var model = products.FirstOrDefault(p => p.Id == id);
            return View(model);
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