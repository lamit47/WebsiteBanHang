using GearBatOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers
{
    [Authorize(Roles = "Administrator, Manager, Employee")]
    public class BrandController : Controller
    {
        Paging pg = new Paging();
        GearBatOnContext _dbContext = new GearBatOnContext();
        // GET: Brand
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PartialBrand(int? page)
        {
            if (page == null) page = 1;
            int take = 10;
            int total = _dbContext.Brands.Where(x => x.Status == true).Count();
            List<Brand> dsBrand = _dbContext.Brands.Where(x => x.Status == true).OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
            ViewBag.Paging = pg.Pagination(total, (int)page, take);

            return PartialView("PartialBrand", dsBrand);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brand brands)
        {
            brands.Status = true;
            _dbContext.Brands.Add(brands);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Brand dsBrand = _dbContext.Brands.FirstOrDefault(x => x.Id == id);
            return View(dsBrand);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string name)
        {
            Brand b = _dbContext.Brands.FirstOrDefault(x => x.Id == id);
            if (b != null)
            {
                b.Name = name;
                b.Status = true;
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            Brand dsBrand = _dbContext.Brands.FirstOrDefault(x => x.Id == id);
            return View(dsBrand);
        }

        public ActionResult Delete(int id)
        {
            Brand brands = _dbContext.Brands.FirstOrDefault(x => x.Id == id);
            if (brands == null)
            {
                return HttpNotFound();
            }
            return View(brands);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(int id)
        {
            Brand brands = _dbContext.Brands.FirstOrDefault(x => x.Id == id);
            brands.Status = false;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}