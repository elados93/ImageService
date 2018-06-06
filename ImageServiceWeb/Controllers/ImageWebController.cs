﻿using ImageServiceWeb.Models;
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


        // GET: ImageWeb
        public ActionResult ImageWeb()
        {
            ViewBag.ConnectedStat = ImageWebModel.ServiceConnected;
            ViewBag.numberOfPhotos = ImageWebModel.NumberOfPhotos;
            return View(ImageWebModel.getStudentsFromFile());
        }
    }
}