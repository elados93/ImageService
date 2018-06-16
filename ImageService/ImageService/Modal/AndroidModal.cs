using ImageService.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class AndroidModal
    {
        private string handler;
        private string outputDir;
        private AndroidTcpClient androidTcpClient;

        public AndroidModal(String oneHandler, String output)
        {
            androidTcpClient = AndroidTcpClient.Instance;
            androidTcpClient.handelPicture += getPicture;
            handler = oneHandler;
            outputDir = output;
        }

        private void getPicture(string picName, byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {        
                checkIfExists(picName);
                File.WriteAllBytes(handler + "\\" + picName, byteArray);
            }
        }

        private void checkIfExists(string picName)
        {
            DirectoryInfo outputD = new DirectoryInfo(outputDir);
            foreach (DirectoryInfo year in outputD.EnumerateDirectories())
            {
                foreach (DirectoryInfo month in year.EnumerateDirectories())
                {
                    foreach (FileInfo file in month.EnumerateFiles())
                    {
                        if (file.Name.Equals(picName))
                        {
                            try
                            {
                                File.Delete(file.FullName);
                                String thumbnailsPath = outputDir + "\\" + "Thumbnails" + "\\"
                                    + year.Name + "\\" + month.Name + "\\" + picName;
                                File.Delete(thumbnailsPath);
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e.Message);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
