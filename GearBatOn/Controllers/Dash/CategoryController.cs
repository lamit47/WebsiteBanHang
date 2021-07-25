using GearBatOn.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers
{
    [Authorize(Roles = "Administrator, Manager, Employee")]
    public class CategoryController : Controller
    {
        Paging pg = new Paging();
        GearBatOnContext _dbContext = new GearBatOnContext();
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialCategory(int? page)
        {
            if (page == null) page = 1;
            int take = 10;
            int total = _dbContext.Categories.Where(x => x.Status == true).Count();
            List<Category> categoriesList = _dbContext.Categories.Where(x => x.Status == true).OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
            ViewBag.Paging = pg.Pagination(total, (int)page, take);

            return PartialView("PartialCategory" ,categoriesList);
        }

        public ActionResult Edit(int Id)
        {
            Category category = _dbContext.Categories.FirstOrDefault(x => x.Id == Id);
            return View(category);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            category.Status = true;
            _dbContext.Categories.AddOrUpdate(category);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public ActionResult Delete(int Id)
        {
            Category category = _dbContext.Categories.FirstOrDefault(x => x.Id == Id);
            category.Status = false;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        public ActionResult Create(Category category)
        {
            Category temp = _dbContext.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (temp == null)
            {
                category.Status = true;
                _dbContext.Categories.AddOrUpdate(category);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}