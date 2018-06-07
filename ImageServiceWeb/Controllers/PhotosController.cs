using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public ActionResult DeletePhoto(string photoView)
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

        public ActionResult AcceptDeletion(string photoView)
        {
            foreach (OnePhoto photo in photoModel.photos)
            {
                if (photo.RelPathWithoutThumb == photoView)
                {
                    try
                    {
                        string thumbnailPhoto = photo.FullPathWithThumbnail;
                        string pathPhoto = photo.FullPath;
                        System.IO.File.Delete(pathPhoto);
                        System.IO.File.Delete(thumbnailPhoto);
                        photoModel.photos.Remove(photo);
                    }
                    catch (Exception e) { Debug.WriteLine(e.Message); }
                    return RedirectToAction("Photos");
                }
            }
            return RedirectToAction("Photos");
        }
    }
}