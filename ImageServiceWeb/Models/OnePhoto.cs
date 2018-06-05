using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class OnePhoto
    {
        private string photoName;
        public int PhotoMonth;
        public int PhotoYear;
        public string RelPath;
        public string Name;
        public string RelPathWithoutThumb;
        public string FullPath;
        public string FullPathWithThumbnail;

        public OnePhoto(string name, int photoMonth, int photoYear, string relPath, string relPathWithoutThumb,
            string fullPath, string fullPathWithThumbnails)
        {
            Name = name;
            PhotoMonth = photoMonth;
            PhotoYear = photoYear;
            RelPath = relPath;
            RelPathWithoutThumb = relPathWithoutThumb;
            FullPath = fullPath;
            FullPathWithThumbnail = fullPathWithThumbnails;
        }



    }
}