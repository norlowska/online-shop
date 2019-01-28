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
using System.Web.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        public ActionResult Index(int? str)
        {
            int i = db.products.Count();
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetUserId();
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(db);
                ApplicationUserManager userManager = new ApplicationUserManager(store);
                ApplicationUser cUser = userManager.FindById(id);
                ViewBag.Ograniczeni = cUser.Ograniczeni;
                ViewBag.pom = cUser.Ograniczeni;
              
                
                if(i>cUser.Ograniczeni)
                {
                    int reszta = i % cUser.Ograniczeni;
                    if(reszta==0)
                    {
                        ViewBag.ilosc_stron = i / cUser.Ograniczeni;
                        
                    }
                    else
                    {
                        ViewBag.ilosc_stron = (i / cUser.Ograniczeni) + 1;
                        
                    }


                    if(str==null || str==1)
                    {
                        ViewBag.i = 0;
                        ViewBag.Ograniczeni = cUser.Ograniczeni;
                    }
                    else
                    {
                        for(int j=2; j<= ViewBag.ilosc_stron; j++)
                        {

                            if(str==j)
                            {
                                ViewBag.i = cUser.Ograniczeni * (j-1);
                                ViewBag.Ograniczeni = cUser.Ograniczeni*j;
                            }

                            if(str== ViewBag.ilosc_stron)
                            {
                                ViewBag.i = cUser.Ograniczeni * (j - 1);
                                ViewBag.Ograniczeni = db.products.Count(); 
                            }

                        }

                    }
                }
                else
                {
                    int limit = 5;
                    int reszta = i % limit;
                    if(reszta==0)
                    {
                        ViewBag.ilosc_stron = i / limit;
                        
                    }
                    else
                    {
                        ViewBag.ilosc_stron = (i / limit) + 1;
                        
                    }


                    if(str==null || str==1)
                    {
                        ViewBag.i = 0;
                        ViewBag.Ograniczeni = limit;
                    }
                    else
                    {
                        for(int j=2; j<= ViewBag.ilosc_stron; j++)
                        {

                            if(str==j)
                            {
                                ViewBag.i = limit * (j-1);
                                ViewBag.Ograniczeni = limit * j;
                            }

                            if(str== ViewBag.ilosc_stron)
                            {
                                ViewBag.i = limit * (j - 1);
                                ViewBag.Ograniczeni = db.products.Count(); 
                            }

                        }

                    }
                   
                }
                
            }
            else
            {
                int limit = 5;
                int reszta = i % limit;
                if (reszta == 0)
                {
                    ViewBag.ilosc_stron = i / limit;

                }
                else
                {
                    ViewBag.ilosc_stron = (i / limit) + 1;

                }


                if (str == null || str == 1)
                {
                    ViewBag.i = 0;
                    ViewBag.Ograniczeni = limit;
                }
                else
                {
                    for (int j = 2; j <= ViewBag.ilosc_stron; j++)
                    {

                        if (str == j)
                        {
                            ViewBag.i = limit * (j - 1);
                            ViewBag.Ograniczeni = limit * j;
                        }

                        if (str == ViewBag.ilosc_stron)
                        {
                            ViewBag.i = limit * (j - 1);
                            ViewBag.Ograniczeni = db.products.Count();
                        }

                    }

                }
            }
           
            ViewBag.str = str;
            var model = db.products.ToList();
            return View(model);
        }

        public ActionResult test(int? str)
        {
            ViewBag.str = str;
            
            return View();
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
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var id_user = User.Identity.GetUserId();

            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(db);
            ApplicationUserManager userManager = new ApplicationUserManager(store);
            ApplicationUser cUser = userManager.FindById(id_user);
            Product product = db.products.Where(p => id == p.Id).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            product.toDictionary();
            return View(product);
        }

        [HttpPost]
        public ActionResult Details(Product p, int quantity)
        {
            return Redirect("/ShoppingCart/AddToCart/" + p.Id + "?qty=" + quantity);
        }

        public ActionResult DownloadFile(String list, int id)
        {

            //string path = AppDomain.CurrentDomain.BaseDirectory + "FolderName/";
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + list;
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                String[] temp = list.Split('/');
                string fileName = temp[temp.Length - 1];
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch(FileNotFoundException)
            {
                Product produkt = db.products.Find(id);
                produkt.toDictionary();
                
                for (int i = 0; i < produkt.files.Count; i++)
                {
                    List<Object> collection = new List<Object>((IEnumerable<Object>)produkt.files[i]);
                    if ((String)collection[0] == list)
                    {
                        produkt.files.RemoveAt(i);
                        produkt.toJson();
                        db.Entry(produkt).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [ChildActionOnly]
        public ActionResult CategoriesMenu()
        {
            var categories = db.categories.ToList();
            return PartialView(categories);
        }
    }
}

