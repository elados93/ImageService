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


        /// <summary>
        /// in case the user chose to see the photo, it moves to another view
        /// that shoes the photo. it loop throght all the photo in the list according
        /// to the model and if it find the photo shows it.
        /// </summary>
        /// <param name="photoView">The photo view.</param>
        /// <returns></returns>
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

        /// <summary>
        /// in case the user wanted to delete the photogo to the view that
        /// verifies the deletion.
        /// </summary>
        /// <param name="photoView">The photo view.</param>
        /// <returns></returns>
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

        /// <summary>
        /// checks if the photo is inside the list, and if do Accepts the deletion.
        /// it delete the photo and the thumbnail photo from the output
        /// directory and return to the photo view.
        /// </summary>
        /// <param name="photoView">The photo view.</param>
        /// <returns></returns>
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