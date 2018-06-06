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
    public class PhotosModel
    {

        private IImageServiceClient imageServiceClient;
        public string OutputDirectory
        {
            get
            {
                return m_outputDirectory;
            }
            set
            {
                m_outputDirectory = value;
                outputUpdate = true;
            }
        }
        private string m_outputDirectory;
        private bool outputUpdate;

        public PhotosModel()
        {
            imageServiceClient = ImageServiceClient.Instance;
            imageServiceClient.UpdateAllModels += getOutputDirFromService;
            outputUpdate = false;
            photos = new List<OnePhoto>();

            if (imageServiceClient.ClientConnected)
            {
                MessageCommand requestAppConfig = new MessageCommand((int)CommandEnum.GetConfigCommand, null, null);
                imageServiceClient.sendCommand(requestAppConfig);
                while (!outputUpdate)
                {
                    Thread.Sleep(100);
                }
                addPhotosToList();
            }
        }

        private void addPhotosToList()
        {
            if (outputUpdate && photos != null)
            {
                string thumbnailDir = m_outputDirectory + "\\Thumbnails";
                if (Directory.Exists(thumbnailDir))
                {
                    DirectoryInfo directory = new DirectoryInfo(thumbnailDir);
                    string[] validExtensions = { ".jpg", ".gif", ".png", ".bmp" };
                    foreach (DirectoryInfo yearDir in directory.GetDirectories())
                    {
                        if (!Path.GetDirectoryName(yearDir.FullName).EndsWith("Thumbnails"))
                        {
                            continue;
                        }
                        foreach (DirectoryInfo monthDir in yearDir.GetDirectories())
                        {
                            foreach (FileInfo fileInfo in monthDir.GetFiles())
                            {
                                // Check if the extention is valid
                                if (validExtensions.Contains(fileInfo.Extension.ToLower()))
                                {
                                    photos.Add(new OnePhoto(
                                        fileInfo.Name,
                                        Int32.Parse(monthDir.Name),
                                        Int32.Parse(yearDir.Name),
                                        "~/" + yearDir.Parent.Parent.Name + "/" + yearDir.Parent.Name + "/" + yearDir.Name + "/" + monthDir.Name + "/" + fileInfo.Name,
                                        "~/" + yearDir.Parent.Parent.Name + "/" + yearDir.Name + "/" + monthDir.Name + "/" + fileInfo.Name,
                                        fileInfo.FullName.Replace(@"Thumbnails\", String.Empty),
                                        fileInfo.FullName)
                                    );
                                }
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }

        public void getOutputDirFromService(MessageCommand msg)
        {
            if (msg.CommandID == (int)CommandEnum.GetConfigCommand)
            {
                OutputDirectory = msg.CommandArgs[1]; // output Directory
            }
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Photos")]
        public List<OnePhoto> photos { get; set; }
    }
}