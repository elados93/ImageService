using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageServiceGUI.ViewModel
{
    /// <summary>
    /// Convert the color that xaml 
    /// </summary>
    public class ConvertLogColor : IValueConverter
    {
        /// <summary>
        /// Function converts the value from string to an object of Brushes.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <param name="targetType">The type to convert.</param>
        /// <param name="parameter">Not critical for now.</param>
        /// <param name="culture">Not critical for now.</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if(targetType.Name != "Brush")
                throw new Exception("Converting only Brush!");
            
            if (value.ToString() == "INFO")
                return Brushes.Green;
            if((value.ToString() == "WARNING"))
                return Brushes.Yellow;
            if ((value.ToString() == "FAIL"))
                return Brushes.Red;

            return Brushes.Transparent;
        }

        // Not importened for now.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
