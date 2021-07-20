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
        GearBatOnContext _dbContext = new GearBatOnContext();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialProduct()
        {
            List<Product> products = _dbContext.Products.ToList();
            return PartialView("PartialProduct", products);
        }
    }
}