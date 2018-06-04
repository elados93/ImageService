using Communication;
using ImageServiceGUI.Communication;
using Infrastracture.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace ImageServiceWeb.Models
{

    public class ImageWebModel
    {
        private static ConfigModel configModel;
        public static IImageServiceClient imageServiceClient;
        private string m_outputDir;

        //public event UpdatePhotos UpdatesAboutPhotos;

        // public static bool ServiceConnected
        // {
        //    get { return imageServiceClient.ClientConnected; }
        // }

        //  public static int NumberOfPhotos;

        public ImageWebModel()
        {
            NumberOfPhotos = -1;
            imageServiceClient = ImageServiceClient.Instance;

            imageServiceClient.UpdateAllModels += getOutputDirFromService;
            MessageCommand requestAppConfig = new MessageCommand((int)CommandEnum.GetConfigCommand, null, null);
            imageServiceClient.sendCommand(requestAppConfig);
        }


        public void getOutputDirFromService(MessageCommand msg)
        {
            if (msg.CommandID == (int)CommandEnum.GetConfigCommand)
            {
                OutputDirectory = msg.CommandArgs[1]; // output Directory
            }
        }

        public static int getNumPhotos(string outputDirectory)
        {
            int countPhotos = 0;
            if (outputDirectory == null || outputDirectory == "")
                return countPhotos;
            else
            {
                DirectoryInfo directoryName = new DirectoryInfo(outputDirectory);
                countPhotos += directoryName.GetFiles("*.jpg", SearchOption.AllDirectories).Length;
                countPhotos += directoryName.GetFiles("*.png", SearchOption.AllDirectories).Length;
                countPhotos += directoryName.GetFiles("*.bmp", SearchOption.AllDirectories).Length;
                countPhotos += directoryName.GetFiles("*.gif", SearchOption.AllDirectories).Length;
                return countPhotos;
            }
        }

        [Required]
        [Display(Name = "Status")]
        public static bool ServiceConnected
        {
            get { return imageServiceClient.ClientConnected; }
        }

        [Required]
        [Display(Name = "Number of Photos")]
        public int NumberOfPhotos { get {
                while (m_numberOfPhotos == -1)
                {
                    Thread.Sleep(100);
                }
                return m_numberOfPhotos;
            } set { m_numberOfPhotos = value; } }

        private int m_numberOfPhotos;

        private string OutputDirectory { get { return m_outputDir; } set
            {
                m_outputDir = value;
                NumberOfPhotos = getNumPhotos(value);
            }
        }
    }
}