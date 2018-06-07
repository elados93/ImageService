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
        #region Members
        private IImageServiceClient imageServiceClient;
        private ConfigSingelton configSingelton;
        #endregion

        public string OutputDirectory { get; set; }

        public PhotosModel()
        {
            imageServiceClient = ImageServiceClient.Instance;
            configSingelton = ConfigSingelton.Instance;
            photos = new List<OnePhoto>();
            OutputDirectory = configSingelton.OutputDirectory;
            addPhotosToList();
        }

        private void addPhotosToList()
        {
            if (photos != null)
            {
                string thumbnailDir = OutputDirectory + "\\Thumbnails";
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

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Photos")]
        public List<OnePhoto> photos { get; set; }
    }
}