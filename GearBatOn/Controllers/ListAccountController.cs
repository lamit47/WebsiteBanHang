using GearBatOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers
{
    public class ListAccountController : Controller
    {
        Paging pg = new Paging();
        GearBatOnContext _dbContext = new GearBatOnContext();
        // GET: ListAccount
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PartialAccount(int? page)
        {
            if (page == null) page = 1;
            int take = 10;
            int total = _dbContext.AspNetUsers.Count();
            List<AspNetUser> dsAcc = _dbContext.AspNetUsers.OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
            ViewBag.Paging = pg.Pagination(total, (int)page, take);

            return PartialView("PartialAccount", dsAcc);
        }
        public ActionResult Edit(string id)
        {
            AspNetUser dsAcc = _dbContext.AspNetUsers.FirstOrDefault(x => x.Id == id);
            return View(dsAcc);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string Email, bool EmailConfirmed, string PhoneNumber, bool PhoneNumberConfirmed, int AccessFailedCount, string UserName, string FullName)
        {
            AspNetUser b = _dbContext.AspNetUsers.FirstOrDefault(x => x.Id == id);
            if (b != null)
            {
                b.Email = Email;
                b.EmailConfirmed = EmailConfirmed;
                b.PhoneNumber = PhoneNumber;
                b.PhoneNumberConfirmed = PhoneNumberConfirmed;
                b.AccessFailedCount = AccessFailedCount;
                b.UserName = UserName;
                b.FullName = FullName;
            }
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}