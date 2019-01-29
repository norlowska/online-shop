using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Product
        public ActionResult Index()
        {
            var m = User.Identity.Name;
            var model = db.products.ToList();
            return View(model);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            var items = db.categories.ToList();
            ViewBag.CategoriesList = items;
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,description,price,cat_pro")]Product product, String fileDescription)
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
                    product.Add_date = DateTime.Now;
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


        // GET: Admin/Product/Edit/{id}
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

        //POST: Admin/Product/Edit
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
                                path = path + fileName.Split('.')[i] + "_smaller." + fileName.Split('.')[i + 1];
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
            return RedirectToAction("Index");
        }

    }
}