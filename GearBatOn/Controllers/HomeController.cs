using GearBatOn.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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

        public ActionResult ListProduct(int? id, string query, string sortBy, decimal? fromPrice, decimal? toPrice, int? page, string type)
        {
            if (query == null) ViewBag.query = "all";
            else ViewBag.query = query;

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

            ViewBag.type = type;

            return View();
        }

        public ActionResult PartialListCategory()
        {
            List<Category> categories = _dbContext.Categories.Where(x => x.Status == true).ToList();
            return PartialView("PartialListCategory", categories);
        }

        public ActionResult PartialListBrand()
        {
            List<Brand> brands = _dbContext.Brands.Where(x => x.Status == true).ToList();
            return PartialView("PartialListBrand", brands);
        }

        public ActionResult PartialListProduct(int id, string query, string sortBy, decimal? fromPrice, decimal? toPrice, int? page, string type)
        {
            if (page == null) page = 1;
            int take = 20;
            var result = GetProduct(id, query, sortBy, fromPrice, toPrice, page, take, type);
            
            ViewBag.Paging = pg.Pagination(result.Item1, (int)page, take);

            return PartialView("PartialListProduct", result.Item2);
        }

        public (int, List<Product>) GetProduct(int id, string query, string sortBy, decimal? fromPrice, decimal? toPrice, int? page, int take, string type)
        {
            List<Product> tempProducts;

            if (id != 0)
            {
                if (type == "brand")
                    tempProducts = _dbContext.Products.Where(x => x.BrandId == id && x.Status == true).ToList();
                else
                    tempProducts = _dbContext.Products.Where(x => x.CategoryId == id && x.Status == true).ToList();
            }
            else
            {
                tempProducts = _dbContext.Products.Where(x => x.Status == true).ToList();
            }

            if (fromPrice != 0 && toPrice != 0)
                tempProducts = tempProducts.Where(x => x.Price >= fromPrice && x.Price <= toPrice).ToList();
            else if (fromPrice != 0 && toPrice == 0)
                tempProducts = tempProducts.Where(x => x.Price >= fromPrice).ToList();
            else if (fromPrice == 0 && toPrice != 0)
                tempProducts = tempProducts.Where(x => x.Price <= toPrice).ToList();

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
                    tempProducts = tempProducts.OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
                    break;
                //case "latest":
                //    tempProducts = tempProducts.OrderByDescending(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
                //    break;
                default:
                    tempProducts = tempProducts.OrderByDescending(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
                    break;
            }

            if (query != "all")
            {
                tempProducts = tempProducts.Where(x => x.Name.ToLower().Contains(query.ToLower())).ToList();
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

        // Controller thêm vào giỏ hàng / thêm 1 model Item
        public ActionResult AddToCart(int Id)
        {
            Product product = _dbContext.Products.Find(Id);
            List<Item> listCart = (Session["cart"] == null) ? new List<Item>() : (List<Item>)Session["cart"];

            if (product != null)
            {
                Item item = listCart.Find(x => x.Product.Id == product.Id);
                if (item == null)
                {
                    listCart.Add(new Item() { Product = product, Quantity = 1 });
                    Session["cart"] = listCart; // lưu list item vào session "cart"
                }
                else
                {
                    listCart.ForEach(x => {
                        if (x.Product.Id == item.Product.Id)
                        {
                            x.Quantity++;
                        }
                    });
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int Id)
        {
            List<Item> listCart = (Session["cart"] == null) ? new List<Item>() : (List<Item>)Session["cart"];
            Item item = listCart.Find(x => x.Product.Id == Id);
            if (item != null)
            {
                listCart.RemoveAll(x => x.Product.Id == item.Product.Id);
                Session["cart"] = listCart;
            }
            return RedirectToAction("Index");
        }

        public ActionResult CheckOut()
        {
            Invoice invoice = new Invoice();
            invoice.countries = _dbContext.Countries.ToList();
            invoice.provinces = _dbContext.Provinces.ToList();
            invoice.promotions = _dbContext.Promotions.ToList();

            return View(invoice);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(Invoice invoice)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            invoice.CustomerId = user.Id;   // gan id theo id khach hang dang nhap
            invoice.Date = DateTime.Now; // lay ngay gio hien tai

            //lưu vào invoicedetail
            List<Item> listCart = (List<Item>)Session["cart"];
            foreach (var item in listCart)
            {
                InvoiceDetail invoiceDetail = new InvoiceDetail();
                invoiceDetail.ProductId = item.Product.Id;
                invoiceDetail.Quantity = item.Quantity;
                invoiceDetail.InvoiceId = invoice.Id;
                _dbContext.InvoiceDetails.Add(invoiceDetail);
            }

            _dbContext.Invoices.Add(invoice);
            _dbContext.SaveChanges();
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCheckOut(int Id)
        {
            List<Item> listCart = (Session["cart"] == null) ? new List<Item>() : (List<Item>)Session["cart"];
            Item item = listCart.Find(x => x.Product.Id == Id);
            if (item != null)
            {
                listCart.RemoveAll(x => x.Product.Id == item.Product.Id);
                Session["cart"] = listCart;
            }
            return RedirectToAction("CheckOut");
        }

        public ActionResult Plus(int Id)
        {
            List<Item> listCart = (Session["cart"] == null) ? new List<Item>() : (List<Item>)Session["cart"];
            Item item = listCart.Find(x => x.Product.Id == Id);
            if (item != null)
            {
                listCart.ForEach(x =>
                {
                    if (x.Product.Id == item.Product.Id)
                    {
                        x.Quantity++;
                    }
                });
                Session["cart"] = listCart;
            }
            return RedirectToAction("CheckOut");
        }

        public ActionResult Minus(int Id)
        {
            List<Item> listCart = (Session["cart"] == null) ? new List<Item>() : (List<Item>)Session["cart"];
            Item item = listCart.Find(x => x.Product.Id == Id);
            if (item != null)
            {
                if (item.Quantity > 1)
                {

                    listCart.ForEach(x =>
                    {
                        if (x.Product.Id == item.Product.Id)
                        {
                            x.Quantity--;
                        }
                    });
                }
                else
                {
                    listCart.RemoveAll(x => x.Product.Id == item.Product.Id);
                }
                Session["cart"] = listCart;
            }
            return RedirectToAction("CheckOut");
        }

        public ActionResult CheckPromoCode(FormCollection collection)
        {
            List<Promotion> promotions = _dbContext.Promotions.ToList();
            string codeinput = collection.Get("codeinput");
            foreach (var item in promotions)
            {
                if (codeinput == item.PromoCode)
                {
                    ViewBag.ratio = item.Ratio;
                }
            }

            return RedirectToAction("CheckOut");
        }
    }
}