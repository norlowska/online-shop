using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        ApplicationDbContext baza = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index()
        {
            var model = baza.Orders.ToList();
            return View(model);
        }

        // GET: Admin/Order/Details/5
        public ActionResult Details(int id)
        {
            var model = baza.OrderDetails.Where(od => od.OrderId == id).ToList();
            ViewBag.order = baza.Orders.Find(id);
            return View(model);
        }

        // GET: Admin/Order/Edit/5
        public ActionResult Edit(int id)
        {
            Order model = baza.Orders.Single(o => o.OrderId == id);         
            return View(model);
        }

        // POST: Admin/Order/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Order o)
        {
            try
            {
                baza.Entry(o).State = EntityState.Modified;
                baza.SaveChanges();
                //TODO: Email do usera
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Order/Delete/5
        public ActionResult Delete(int id)
        {
            var model = baza.Orders.Find(id);
            return View(model);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Order o)
        {
            try
            {
                baza.Orders.Remove(o);
                baza.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
