using GearBatOn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers
{
    [Authorize(Roles = "Administrator, Manager, Employee")]
    public class PromotionController : Controller
    {
        GearBatOnContext context =new GearBatOnContext();
        Paging pg = new Paging();

        // GET: Promotion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PartialPromo(int? page)
        {
            if (page == null) page = 1;
            int take = 10;
            int total = context.Promotions.Count();
            List<Promotion> dsPromo = context.Promotions.Where(x=>x.Status==true).OrderBy(x => x.Id).Skip(((int)page - 1) * take).Take(take).ToList();
            ViewBag.Paging = pg.Pagination(total, (int)page, take);

            return PartialView("PartialPromo", dsPromo);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Promotion promo)
        {
            promo.Status = true;
            context.Promotions.Add(promo);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Promotion promo = context.Promotions.FirstOrDefault(x => x.Id == id);
            return View(promo);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string name, string promoCode, double ratio,bool status)
        {
            Promotion b = context.Promotions.FirstOrDefault(x => x.Id == id);
            if (b != null)
            {
                b.Name = name;
                b.PromoCode = promoCode;
                b.Ratio = ratio;
                b.Status = status;
            }
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            Promotion dsPromo = context.Promotions.FirstOrDefault(x => x.Id == id);
            return View(dsPromo);
        }

        public ActionResult Delete(int id)
        {
            Promotion promo = context.Promotions.FirstOrDefault(x => x.Id == id);
            if (promo == null)
            {
                return HttpNotFound();
            }
            return View(promo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(int id)
        {
            Promotion promo = context.Promotions.FirstOrDefault(x => x.Id == id);
            promo.Status = false;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}