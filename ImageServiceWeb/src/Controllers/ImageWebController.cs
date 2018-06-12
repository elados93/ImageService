using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class ImageWebController : Controller
    {

        private static ImageWebModel imageWebModel;

        public ImageWebController()
        {
            imageWebModel = new ImageWebModel();
        }


        /// <summary>
        /// gets the information about the image web 
        /// model, about the connection to the service and the number of photos
        /// and get the information about the students.
        /// </summary>
        /// <returns></returns>
        public ActionResult ImageWeb()
        {
            ViewBag.ConnectedStat = ImageWebModel.ServiceConnected;
            ViewBag.numberOfPhotos = ImageWebModel.NumberOfPhotos;
            return View(ImageWebModel.getStudentsFromFile());
        }
    }
}