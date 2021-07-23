using GearBatOn.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers
{
    [Authorize(Roles = "Administrator, Manager, Employee")]
    public class ImageController : Controller
    {
        GearBatOnContext _dbContext = new GearBatOnContext();
        // GET: Image
        public ActionResult Index(int id)
        {
            List<Image> images = _dbContext.Images.Where(x => x.ProductId == id).ToList();
            ViewBag.Id = id;
            return View(images);
        }

        public ActionResult Add(int id)
        {
            Image image = new Image();
            image.ProductId = id;
            return View(image);
        }

        [HttpPost]
        public ActionResult Add(Image imageModel)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageModel.ImageFile.FileName);
            imageModel.ImagePath = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            _dbContext.Images.Add(imageModel);
            _dbContext.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("Add", new { id = imageModel.ProductId });
        }

        public ActionResult Delete(int id)
        {
            Image image = _dbContext.Images.FirstOrDefault(x => x.Id == id);
            var temp = image.ProductId;
            string fullPath = Request.MapPath(image.ImagePath);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            _dbContext.Images.Remove(image);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", new { id = temp });
        }
    }
}