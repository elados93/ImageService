using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class PhotosController : Controller
    {

        private static PhotosModel photoModel;

        public PhotosController()
        {
            photoModel = new PhotosModel();
        }


        // GET: Photos
        public ActionResult Photos()
        {
            return View(photoModel.photos);
        }

        public ActionResult PhotoView(string photoView)
        {
            foreach (OnePhoto photo in photoModel.photos)
            {
                if (photo.RelPathWithoutThumb == photoView)
                {
                    return View(photo);
                }
            }
            return View();
        }

        public ActionResult DeleteImage(string photoView)
        {
            foreach (OnePhoto photo in photoModel.photos)
            {
                if (photo.RelPathWithoutThumb == photoView)
                {
                    return View(photo);
                }
            }
            return View();
        }
    }
}