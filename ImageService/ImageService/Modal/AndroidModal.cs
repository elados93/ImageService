using ImageService.Communication;
using System;
using System.Collections.Generic;
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
        private AndroidTcpClient androidTcpClient;

        public AndroidModal(String oneHandler)
        {
            androidTcpClient = AndroidTcpClient.Instance;
            androidTcpClient.handelPicture += getPicture;
            handler = oneHandler;
        }

        private void getPicture(byte[] responseObj)
        {
            ImageConverter imgConvertor = new ImageConverter();
            Image image = (Image)imgConvertor.ConvertFrom(responseObj);
            image.Save(handler);
            //Bitmap b = new Bitmap(image);
        }
    }
}
