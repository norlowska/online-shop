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
   
        /// <summary>
        ///  GET: Admin/Order
        /// </summary>
        /// <returns> List Order</returns>
        public ActionResult Index()
        {
            var model = baza.Orders.ToList();
            return View(model);
        }

         
        /// <summary>
        /// GET: Admin/Order/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Details Order</returns>
        public ActionResult Details(int id)
        {
            var model = baza.OrderDetails.Where(od => od.OrderId == id).ToList();
            ViewBag.order = baza.Orders.Find(id);
            return View(model);
        }

        // 
        /// <summary>
        /// GET: Admin/Order/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Edit Order </returns>
        public ActionResult Edit(int id)
        {
            Order model = baza.Orders.Single(o => o.OrderId == id);         
            return View(model);
        }

        // POST: Admin/Order/Edit/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="o"></param>
        /// <returns></returns>
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

        
        /// <summary>
        /// GET: Admin/Order/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Delet Order</returns>
        public ActionResult Delete(int id)
        {
            var model = baza.Orders.Find(id);
            return View(model);
        }


        /// <summary>
        ///    POST: Admin/Order/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="o"></param>
        /// <returns>Redirect To Action </returns>
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
