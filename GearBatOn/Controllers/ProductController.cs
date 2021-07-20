using GearBatOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers
{

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
            int take = 10;
            int total = _dbContext.Products.Count();
            List<Product> products = _dbContext.Products.OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
            ViewBag.Paging = pg.Pagination(total, (int)page, take);

            return PartialView("PartialProduct", products);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
            return View(product);
        }

        public ActionResult Details(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
            return View(product);
        }

        public ActionResult Delete(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
            return View(product);
        }
    }
}