using ImageServiceGUI.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{

    public class ImageWebModel
    {

        public static IImageServiceClient imageServiceClient;
        public static bool ServiceConnected
        {
            get { return imageServiceClient.ClientConnected; }
        }
        
        public ImageWebModel()
        {
            imageServiceClient = ImageServiceClient.Instance;
        }


    }
}