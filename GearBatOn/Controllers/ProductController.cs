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
            //if (page == null) page = 1;
            //int take = 10;
            //int total = _dbContext.Products.Count();
            //List<Product> products = _dbContext.Products.OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
            //ViewBag.Paging = pg.Pagination(total, (int)page, take, "Product", "Index", "");

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
    }
}