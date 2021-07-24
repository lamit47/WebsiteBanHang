using GearBatOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers.Dash
{
    public class StatisticController : Controller
    {
        GearBatOnContext _dbContext = new GearBatOnContext();
        // GET: Statistic
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialStatistic(string type)
        {
            DateTime dateTime = DateTime.Now;
            if (type == "week")
                dateTime = dateTime.AddDays(-7);
            else if (type == "month")
                dateTime = dateTime.AddMonths(-1);
            else if (type == "quarterly")
                dateTime = dateTime.AddMonths(-3);
            else if (type == "year")
                dateTime = dateTime.AddYears(-1);

            var list = from a in _dbContext.InvoiceDetails
                       where a.Invoice.Date >= dateTime && a.Invoice.Date <= DateTime.Now
                       group a by a.ProductId;
            List<int> id = new List<int>();
            List<int> total = new List<int>();
            foreach (var group in list)
            {
                foreach (var item in group)
                {
                    id.Add(item.Id);
                    total.Add(item.Quantity);
                }
            }

            List<Product> products = _dbContext.Products.Where(x => id.Contains(x.Id)).ToList();
            int i = 0;
            decimal totalPrice = 0;
            foreach (var item in products)
            {
                Image temp = _dbContext.Images.FirstOrDefault(x => x.ProductId == item.Id);
                item.FeatureImage = temp.ImagePath;
                item.TotalSeller = total[i];
                totalPrice += item.Price * total[i];
                i++;
            }
            products = products.OrderByDescending(x => x.TotalSeller).Take(10).ToList();

            ViewBag.total = totalPrice;
            return PartialView("PartialStatistic", products);
        }
    }
}