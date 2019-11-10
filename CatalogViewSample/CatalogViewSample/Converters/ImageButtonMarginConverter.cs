using System;
using System.Globalization;
using Xamarin.Forms;

namespace CatalogViewSample.Converters
{
    public class ImageButtonMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;
            if (parameter == null)
                return new Rectangle(1, 0, 35, 35);
            else
                return new Rectangle((double)parameter, -1, 35, 35);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
