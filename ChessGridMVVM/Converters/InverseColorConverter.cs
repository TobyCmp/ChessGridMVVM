using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ChessGridMVVM.Converters
{
    public class InverseColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorName = value as string;
            if (colorName == "Black")
                return Brushes.White;
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
