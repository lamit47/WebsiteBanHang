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
    public class InvoiceController : Controller
    {
        Paging pg = new Paging();
        GearBatOnContext _dbContext = new GearBatOnContext();
        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialInvoice(int? page)
        {
           
               if (page == null) page = 1;
               int take = 10;
               int total = _dbContext.Invoices.Count();
               List<Invoice> invoices = _dbContext.Invoices.OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
               foreach (var item in invoices)
               {
                                ApplicationUser staff = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(item.StaffId);
                                ApplicationUser customer = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(item.CustomerId);
                                item.NameStaff = staff.FullName;
                                item.NameCustomer = customer.FullName;
                }
                ViewBag.Paging = pg.Pagination(total, (int)page, take);
                return PartialView("PartialInvoice", invoices);
 
        }
      


        public ActionResult Details(int? id)
        {
            List<InvoiceDetail> invoiceDetails = _dbContext.InvoiceDetails.Where(c => c.InvoiceId == id).ToList();
            //var tgia = from a in _dbContext.InvoiceDetails
            //              from b in _dbContext.Products
            //              where a.ProductId == b.Id
            //              where a.InvoiceId == id
            //              select new { tg = (a.Amount*b.Price) };
            //decimal tonggia = tgia.Sum(c => c.tg);
            foreach (var item in invoiceDetails)
            {
                int x = item.Id;
                var tgsp = from a in _dbContext.InvoiceDetails
                           from b in _dbContext.Products
                           where a.ProductId == b.Id
                           where a.InvoiceId == id
                           where a.Id == x
                           select new { tg = (a.Amount * b.Price) };
                decimal tgspham = tgsp.Sum(c => c.tg);
                item.TotalPrice = tgspham;
            }
            return View(invoiceDetails);
        }
       
        [HttpPost]
        public JsonResult ChangeStatus(int? id)
        {
            var invoice = _dbContext.Invoices.Find(id);
            invoice.PaymentStatus = !invoice.PaymentStatus;
            _dbContext.SaveChanges();
            return Json(new
            {
                status = !invoice.PaymentStatus
            });
            
        }
    }
    
}