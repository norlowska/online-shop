using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class StatsController : Controller
    {
        // GET: Stats
        public ActionResult Index()
        {
            var model = new List<Product>();
            using (ApplicationDbContext baza = new ApplicationDbContext())
            {
                var products = new Dictionary<int, int>(); //key - id produktu  value - ilość
                foreach(var item in baza.products.ToList())
                {
                    int count = 0;
                    foreach(var it in baza.OrderDetails.Where(od => od.Product == item).ToList())
                    {
                        count += it.Quantity;
                    }
                    products.Add(item.Id, count);
                }
                
                for(int i = 0; i< 5; i++)
                {
                    model.Add(baza.products.Single(p => p.Id == products.Max().Key));
                    products.Remove(products.Max().Key);
                }
            }
            return View(model);
        }
    }
}