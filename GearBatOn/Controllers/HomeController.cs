using GearBatOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers
{
    public class HomeController : Controller
    {
        Paging pg = new Paging();
        GearBatOnContext _dbContext = new GearBatOnContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
            product.ListImage = _dbContext.Images.Where(x => x.ProductId == product.Id).ToList();

            return View(product);
        }

        public ActionResult ListProduct(int? id)
        {
            ViewBag.categoryId = id;
            return View();
        }

        public ActionResult PartialHomeProduct(int id)
        {
            List<Product> products = _dbContext.Products.Where(x => x.CategoryId == id && x.Status == true).OrderBy(x => x.Id).Skip(0).Take(10).ToList();
            foreach (var item in products)
            {
                var temp = _dbContext.Images.FirstOrDefault(x => x.ProductId == item.Id);
                if (temp != null)
                {
                    item.FeatureImage = temp.ImagePath;
                }
            }
            return PartialView("PartialHomeProduct", products);
        }

        public ActionResult PartialListProduct(int id, int? page)
        {
            if (page == null) page = 1;
            int take = 10;
            int total = _dbContext.Products.Where(x => x.CategoryId == id && x.Status == true).Count();
            List<Product> products = _dbContext.Products.Where(x => x.CategoryId == id && x.Status == true).OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
            foreach (var item in products)
            {
                var temp = _dbContext.Images.FirstOrDefault(x => x.ProductId == item.Id);
                if (temp != null)
                {
                    item.FeatureImage = temp.ImagePath;
                }
            }
            ViewBag.Paging = pg.Pagination(total, (int)page, take);

            return PartialView("PartialListProduct", products);
        }
    }
}