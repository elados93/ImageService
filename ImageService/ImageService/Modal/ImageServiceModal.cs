using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageService.Modal
{
    class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string outputFolder; 
        private int thumbnailSize; 
        #endregion 

        public ImageServiceModal(string outputFolderArg, int thumbnailSizeArg)
        {
            outputFolder = outputFolderArg;
            thumbnailSize = thumbnailSizeArg;
        }

        public string AddFile(string path, out bool result)
        {
            try
            {
                string month = string.Empty;
                string year = string.Empty;
                string thumbnailsPath = outputFolder + "\\Thumbnails";
                if (File.Exists(path))
                {
                    DateTime date = File.GetCreationTime(path);
                    month = date.Month.ToString();
                    year = date.Year.ToString();
                    // create the directory that the photo will be in it.
                    Directory.CreateDirectory(outputFolder);
                    Directory.CreateDirectory(thumbnailsPath);
                    Directory.CreateDirectory(outputFolder + "\\" + year);
                    //create folders for each month.
                    Directory.CreateDirectory(outputFolder + "\\" + year + "\\" + month);
                    Directory.CreateDirectory(thumbnailsPath + "\\" + year + "\\" + month);

                    string outputFolderPath = outputFolder + "\\" + year + "\\" + month;
                    string outputFolderPathThumbnails = thumbnailsPath + "\\" + year + "\\" + month;

                    File.Copy(path, outputFolderPath);

                    Image image = Image.FromFile(path);
                    image = resizeImage(image, new Size(thumbnailSize, thumbnailSize));
                    image.Save(outputFolderPathThumbnails + "\\" + Path.GetFileName(path));

                    result = true;
                    return "Added file successfuly at: " + outputFolderPath;
                }
                else
                {
                    result = false;
                    string noSuchPath = "Not A Valid Image";
                    return noSuchPath;
                }
            }
            catch
            {
                throw new Exception("file does not exists");
            }

        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        public string AddDirectory(string path, out bool result)
        {

        }
    }
}