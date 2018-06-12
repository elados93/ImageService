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

        /// <summary>
        /// constroctor
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="photoMonth">The photo month.</param>
        /// <param name="photoYear">The photo year.</param>
        /// <param name="relPath">The relative path.</param>
        /// <param name="relPathWithoutThumb">The relative path without thumb.</param>
        /// <param name="fullPath">The full path.</param>
        /// <param name="fullPathWithThumbnails">The full path with thumbnails.</param>
        public OnePhoto(string name, int photoMonth, int photoYear, string relPath, string relPathWithoutThumb,
            string fullPath, string fullPathWithThumbnails)
        {
            photoName = name;
            PhotoMonth = photoMonth;
            PhotoYear = photoYear;
            RelPath = relPath;
            RelPathWithoutThumb = relPathWithoutThumb;
            FullPath = fullPath;
            FullPathWithThumbnail = fullPathWithThumbnails;
        }
    }
}