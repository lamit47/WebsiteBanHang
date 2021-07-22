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
        GearBatOnContext _dbContext = new GearBatOnContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialHomeProduct(int id)
        {
            List<Product> products = _dbContext.Products.Where(x => x.CategoryId == id).OrderBy(x => x.Id).Skip(0).Take(10).ToList();
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
    }
}