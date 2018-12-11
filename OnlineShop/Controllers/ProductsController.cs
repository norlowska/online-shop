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

namespace OnlineShop.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        public ActionResult Index()
        {
            var model = db.products.ToList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index_admin()
        {
            var m = User.Identity.Name;
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
            product.toDictionary();
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var items = db.categories.ToList();
            ViewBag.CategoriesList = items;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,description,price,cat_pro.Id")]Product product, String fileDescription)
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
                product.toDictionary();
                if (ModelState.IsValid)
                {

                    if (product != null)
                    {
                        if (Request.Files.Count > 0)
                        {
                            HttpPostedFileBase image = Request.Files["filePath"];


                            for (int i = 1; i < Request.Files.Count; i++)
                            {
                                HttpPostedFileBase file = Request.Files[i];

                                if (file.ContentLength > 0)
                                {
                                    var fileName = Path.GetFileName(file.FileName);

                                    fileName = fileName.Replace(' ', '_');
                                    var filePath = Path.Combine(
                                        Server.MapPath("~/Content/Files/"), fileName);
                                    file.SaveAs(filePath);
                                    List<String> list = new List<String>();
                                    list.Add("/Content/Files/" + fileName);
                                    list.Add(fileDescription);
                                    product.files.Add(list);


                                }
                            }

                            if (image.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(image.FileName);

                                fileName = fileName.Replace(' ', '_');
                                var filePath = Path.Combine(
                                    Server.MapPath("~/Content/Images/"), fileName);
                                image.SaveAs(filePath);
                                product.image["normal"] = "/Content/Images/" + fileName;


                                WebImage imageSmaller = new WebImage(image.InputStream);

                                double ratio = (double)imageSmaller.Height / (double)imageSmaller.Width;

                                while (imageSmaller.Width > 100 && imageSmaller.Height > 100)
                                {
                                    if (imageSmaller.Width > imageSmaller.Height)
                                    {
                                        imageSmaller.Resize(100, (int)Math.Round(100 * ratio));
                                    }

                                    if (imageSmaller.Height > imageSmaller.Width)
                                    {
                                        imageSmaller.Resize((int)Math.Round(100 / ratio), 100);
                                    }
                                }
                                var name = fileName.Split('.');
                                var path = "~/Content/Images/";
                                var pathSave = "/Content/Images/";
                                for (int i = 0; i < name.Length; i++)
                                {

                                    if (i < name.Length - 2)
                                    {
                                        path = path + name[i] + '.';
                                        pathSave = pathSave + name[i] + '.';
                                    }
                                    else
                                    {
                                        path = path + fileName.Split('.')[i] + "_smaller." + fileName.Split('.')[i + 1];
                                        pathSave = pathSave + fileName.Split('.')[i] + "_smaller." + fileName.Split('.')[++i];
                                    }

                                }
                                imageSmaller.Save(path);
                                product.image["smaller"] = pathSave;

                            }
                            product.toJson();
                        }

                    }

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
            produkt.toDictionary();
            return View(produkt);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, String fileDescription)
        {
            if (product != null)
            {
                product.toDictionary();
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase image = Request.Files["filePath"];


                    for (int i = 1; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];

                        if (file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);

                            fileName = fileName.Replace(' ', '_');
                            var filePath = Path.Combine(
                                Server.MapPath("~/Content/Files/"), fileName);
                            file.SaveAs(filePath);
                            List<String> list = new List<String>();
                            list.Add("/Content/Files/" + fileName);
                            list.Add(fileDescription);
                            product.files.Add(list);


                        }
                    }

                    if (image.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(image.FileName);

                        fileName = fileName.Replace(' ', '_');
                        var filePath = Path.Combine(
                            Server.MapPath("~/Content/Images/"), fileName);
                        image.SaveAs(filePath);
                        product.image["normal"] = "/Content/Images/" + fileName;


                        WebImage imageSmaller = new WebImage(image.InputStream);

                        double ratio = (double)imageSmaller.Height / (double)imageSmaller.Width;

                        while (imageSmaller.Width > 100 && imageSmaller.Height > 100)
                        {
                            if (imageSmaller.Width > imageSmaller.Height)
                            {
                                imageSmaller.Resize(100, (int)Math.Round(100 * ratio));
                            }

                            if (imageSmaller.Height > imageSmaller.Width)
                            {
                                imageSmaller.Resize((int)Math.Round(100 / ratio), 100);
                            }
                        }
                        var name = fileName.Split('.');
                        var path = "~/Content/Images/";
                        var pathSave = "/Content/Images/";
                        for (int i = 0; i < name.Length; i++)
                        {

                            if (i < name.Length - 2)
                            {
                                path = path + name[i] + '.';
                                pathSave = pathSave + name[i] + '.';
                            }
                            else
                            {
                                path = path + fileName.Split('.')[i] + "_smaller." + fileName.Split('.')[i+1];
                                pathSave = pathSave + fileName.Split('.')[i] + "_smaller." + fileName.Split('.')[++i];
                            }

                        }
                        imageSmaller.Save(path);
                        product.image["smaller"] = pathSave;

                    }
                    product.toJson();
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var prduktToUpdate = db.products.Find(product.Id);
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
            return View(prduktToUpdate);
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

        [ChildActionOnly]
        public ActionResult CategoriesMenu()
        {
            var categories = db.categories.ToList();
            return PartialView(categories);
        }
    }
}