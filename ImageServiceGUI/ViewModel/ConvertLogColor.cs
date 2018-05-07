using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI.ViewModel
{
    class ConvertLogColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if(targetType.Name != "Brush")
            {
                throw new Exception("Converting only Brush!");
            }

            if (value.ToString() == "INFO")
                return System.Windows.Media.Brushes.Green;
            if((value.ToString() == "WARNING"))
                return System.Windows.Media.Brushes.Yellow;
            if ((value.ToString() == "FAIL"))
                return System.Windows.Media.Brushes.Red;

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
