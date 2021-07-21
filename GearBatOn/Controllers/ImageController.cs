using GearBatOn.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GearBatOn.Controllers
{
    public class ImageController : Controller
    {
        GearBatOnContext _dbContext = new GearBatOnContext();
        // GET: Image
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
            return RedirectToAction("Add", new { id = imageModel.Id });
        }
    }
}