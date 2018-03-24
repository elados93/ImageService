using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class ImageServiceModal : IImageServiceModal
    {
        private string outputFolder;


        public string AddFile(string path, out bool result)
        {
            try
            {

                string month = string.Empty;
                string year = string.Empty;
                if (File.Exists(path))
                {
                    DateTime date = File.GetCreationTime(path);
                    month = date.Month.ToString();
                    year = date.Year.ToString();
                    // create the directory that the photo will be in it.
                    Directory.CreateDirectory(outputFolder);
                    Directory.CreateDirectory(outputFolder + "\\" + year);
                    //create folders for each month.
                    for (int i = 1; i <= 12; i++)
                    {
                        Directory.CreateDirectory(outputFolder + "\\" + year + "\\" + i.ToString());
                    }
                    string folderOfImage = outputFolder + "\\" + year + "\\" + month + "\\";
                    File.Copy(path, folderOfImage + Path.GetFileName(path));
                    result = true;
                    return string.Empty;
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
    }
}