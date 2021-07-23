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

        public ActionResult ListProduct(int? id, string sortBy, decimal? fromPrice, decimal? toPrice, int? page)
        {
            if (id == null) {
                ViewBag.categoryId = 0;
                ViewBag.sortBy = "latest";
                ViewBag.fromPrice = 0;
                ViewBag.toPrice = 0;
                ViewBag.page = 1;
                return View();
            }
            else ViewBag.categoryId = id;

            if (sortBy == null) ViewBag.sortBy = "latest";
            else ViewBag.sortBy = sortBy;

            if (fromPrice == null) ViewBag.fromPrice = 0;
            else ViewBag.fromPrice = fromPrice;

            if (toPrice == null) ViewBag.toPrice = 0;
            else ViewBag.toPrice = toPrice;

            if (page == null) ViewBag.page = 1;
            else ViewBag.page = page;

            return View();
        }

        public ActionResult PartialListProduct(int id, string sortBy, decimal? fromPrice, decimal? toPrice, int? page)
        {
            if (page == null) page = 1;
            int take = 20;
            var result = GetProduct(id, sortBy, fromPrice, toPrice, page, take);
            
            ViewBag.Paging = pg.Pagination(result.Item1, (int)page, take);

            return PartialView("PartialListProduct", result.Item2);
        }

        public (int, List<Product>) GetProduct(int id, string sortBy, decimal? fromPrice, decimal? toPrice, int? page, int take)
        {
            List<Product> tempProducts;

            if ((fromPrice != null && toPrice != null) && (fromPrice != 0 && toPrice != 0))
            {
                tempProducts = _dbContext.Products.Where(x => x.CategoryId == id && x.Status == true && (x.Price >= fromPrice && x.Price <= toPrice)).ToList();
            } 
            else
            {
                tempProducts = _dbContext.Products.Where(x => x.CategoryId == id && x.Status == true).ToList();
            }
            int count = tempProducts.Count();

            switch (sortBy)
            {
                case "ascending":
                    tempProducts = tempProducts.OrderBy(x => x.Price).Skip(((int)page - 1) * take).Take(take).ToList();
                    break;
                case "descending":
                    tempProducts = tempProducts.OrderByDescending(x => x.Price).Skip(((int)page - 1) * take).Take(take).ToList();
                    break;
                case "az":
                    tempProducts = tempProducts.OrderBy(x => x.Name).Skip(((int)page - 1) * take).Take(take).ToList();
                    break;
                case "za":
                    tempProducts = tempProducts.OrderByDescending(x => x.Name).Skip(((int)page - 1) * take).Take(take).ToList();
                    break;
                case "oldest":
                    tempProducts = tempProducts.OrderByDescending(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
                    break;
                //case "latest":
                //    tempProducts = tempProducts.OrderByDescending(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
                //    break;
                default:
                    tempProducts = tempProducts.OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
                    break;
            }

            foreach (var item in tempProducts)
            {
                var temp = _dbContext.Images.FirstOrDefault(x => x.ProductId == item.Id);
                if (temp != null)
                {
                    item.FeatureImage = temp.ImagePath;
                }
            }

            return (count, tempProducts);
        } 
    }
}