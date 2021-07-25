using GearBatOn.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers
{
    [Authorize(Roles = "Administrator, Manager, Employee")]
    public class ProductController : Controller
    {
        Paging pg = new Paging();
        GearBatOnContext _dbContext = new GearBatOnContext();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialProduct(int? page)
        {
            if (page == null) page = 1;
            int take = 5;
            int total = _dbContext.Products.Where(x => x.Status == true).Count();
            List<Product> products = _dbContext.Products.Where(x => x.Status == true).OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
            foreach (var item in products)
            {
                var temp = _dbContext.Images.FirstOrDefault(x => x.ProductId == item.Id);
                if (temp != null)
                {
                    item.FeatureImage = temp.ImagePath;
                }
            }
            ViewBag.Paging = pg.Pagination(total, (int)page, take);

            return PartialView("PartialProduct", products);
        }

        public ActionResult Create()
        {
            Product product = new Product();
            product.ListCategory = _dbContext.Categories.ToList();
            product.ListCountry = _dbContext.Countries.ToList();
            product.ListBrand = _dbContext.Brands.ToList();

            return View(product);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            product.Status = true;
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
            product.ListCategory = _dbContext.Categories.ToList();
            product.ListCountry = _dbContext.Countries.ToList();
            product.ListBrand = _dbContext.Brands.ToList();

            return View(product);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            Product temp = _dbContext.Products.FirstOrDefault(x => x.Id == product.Id);
            if (temp == null)
            {
                return HttpNotFound();
            }
            product.Status = true;
            _dbContext.Products.AddOrUpdate(product);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
            product.ListImage = _dbContext.Images.Where(x => x.ProductId == id).ToList();
            return View(product);
        }

        public ActionResult Delete(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            product.Status = false;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}