﻿using GearBatOn.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers
{
    [Authorize(Roles = "Administrator, Manager, Employee")]
    public class InvoiceController : Controller
    {
        Paging pg = new Paging();
        GearBatOnContext _dbContext = new GearBatOnContext();
        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialInvoice(int? page, bool status)
        {
               if (page == null) page = 1;
               int take = 10;
               int total = _dbContext.Invoices.Count();
               List<Invoice> invoices = _dbContext.Invoices.Where(x => x.PaymentStatus == status)
                .OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
               foreach (var item in invoices)
               {
                    ApplicationUser customer = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(item.CustomerId);
                    item.NameCustomer = customer.FullName;
                }
                ViewBag.Paging = pg.Pagination(total, (int)page, take);
                return PartialView("PartialInvoice", invoices);
        }
      
        public ActionResult Details(int? id)
        {
            List<InvoiceDetail> invoiceDetails = _dbContext.InvoiceDetails.Where(c => c.InvoiceId == id).ToList();
            return View(invoiceDetails);
        }
       
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var invoice = _dbContext.Invoices.FirstOrDefault(x => x.Id == id);
            invoice.PaymentStatus = !invoice.PaymentStatus;
            _dbContext.SaveChanges();
            return Json(new
            {
                status = invoice.PaymentStatus
            });
            
        }
    }
    
}