using GearBatOn.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers
{
    [Authorize(Roles = "Administrator")]
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
            List<AspNetUser> users = _dbContext.AspNetUsers.OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            foreach (var item in users)
            {
                item.RoleName = UserManager.GetRoles(item.Id).FirstOrDefault();
            }

            ViewBag.Paging = pg.Pagination(total, (int)page, take);
            return PartialView("PartialAccount", users);
        }
        public ActionResult Edit(string id)
        {
            AspNetUser account = _dbContext.AspNetUsers.FirstOrDefault(x => x.Id == id);
            return View(account);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AspNetUser user)
        {
            AspNetUser b = _dbContext.AspNetUsers.FirstOrDefault(x => x.Id == user.Id);
            if (b != null)
            {
                _dbContext.AspNetUsers.AddOrUpdate(user);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Role(string id)
        {
            AspNetUser roles = _dbContext.AspNetUsers.FirstOrDefault(x => x.Id == id);
            roles.ListRoles = _dbContext.AspNetRoles.ToList();
            return View(roles);
        }

        [HttpPost, ActionName("Role")]
        public ActionResult Role(string id, string role)
        {
            SetRole(id, role);
            return RedirectToAction("Index");
        }

        public void SetRole(string id, string role)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roles = UserManager.GetRoles(id);
            UserManager.RemoveFromRoles(id, roles.ToArray());
            UserManager.AddToRole(id, role);
        }
    }
}