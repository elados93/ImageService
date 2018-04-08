using System;
using System.Drawing;
using System.IO;

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

        /// <summary>
        /// this function handels the path of the picture and translate the specific date time of the 
        /// creation of the picture.
        /// it creates a proper hidden folder that is orgenized by the years and month of the current
        /// pictures and the future pictures to be.
        /// it states if the action was successful or not and sends a proper massage to the logging.
        /// </summary>
        /// <param name="path"> is the path to handle.</param>
        /// <param name="result"> is the result</param>
        /// <returns></returns>
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
                    // create the directory that the photo will be in it, so it would be a hidden directory.
                    Directory.CreateDirectory(outputFolder).Attributes |= FileAttributes.Hidden;

                    Directory.CreateDirectory(thumbnailsPath);
                    Directory.CreateDirectory(outputFolder + "\\" + year);

                    //create folders for each month.
                    Directory.CreateDirectory(outputFolder + "\\" + year + "\\" + month);
                    Directory.CreateDirectory(thumbnailsPath + "\\" + year + "\\" + month);

                    string outputFolderPath = outputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path);
                    string outputFolderPathThumbnails = thumbnailsPath + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path);

                    // Check for existence of the file already in the output folder.
                    int i = 1;
                    if (File.Exists(outputFolderPath))
                    {
                        do
                        {
                            outputFolderPath = outputFolder + "\\" + year + "\\" + month + "\\" + Path.GetFileNameWithoutExtension(path) + "(" + i + ")" + Path.GetExtension(path);
                            outputFolderPathThumbnails = thumbnailsPath + "\\" + year + "\\" + month + "\\" + Path.GetFileNameWithoutExtension(path) + "(" + i + ")" + Path.GetExtension(path);
                            ++i;
                        } while (File.Exists(outputFolderPath));
                    }

                    // Copy the original photo to the hidden directory.
                    File.Copy(path, outputFolderPath, true);

                    // Thumblizing the picture.
                    Image image = Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
                    image = resizeImage(image, new Size(thumbnailSize, thumbnailSize));
                    image.Save(outputFolderPathThumbnails);
                    image.Dispose();

                    result = true;
                    return "Added file successfuly at: " + outputFolderPath;
                }
                else
                {
                    // If there is no image return false and an appropriate message.
                    result = false;
                    string noSuchPath = "Not A Valid Image";
                    return noSuchPath;
                }
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }

        }

        /// <summary>
        /// this function thumblizes the picture.
        /// </summary>
        /// <param name="imgToResize"> is the image to resize </param>
        /// <param name="size"> is the wanted size to resize</param>
        /// <returns></returns>
        private static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

    }
}