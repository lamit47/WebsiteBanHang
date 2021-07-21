using GearBatOn.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers
{
    public class CategoryController : Controller
    {
        GearBatOnContext _dbContext = new GearBatOnContext();
        // GET: Category
        public ActionResult Index()
        {
            List<Category> categoriesList = _dbContext.Categories.Where(x => x.Status == true).ToList();
            return View(categoriesList);
        }

        public ActionResult PartialCategory()
        {
            List<Category> categoriesList = _dbContext.Categories.Where(x => x.Status == true).ToList();
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
            return RedirectToAction("CategoryList");
        }

        
        public ActionResult Delete(int Id)
        {
            Category category = _dbContext.Categories.FirstOrDefault(x => x.Id == Id);
            category.Status = false;
            _dbContext.SaveChanges();
            return RedirectToAction("CategoryList");
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
                return RedirectToAction("CategoryList");
            }
            return View();
        }

    }
}