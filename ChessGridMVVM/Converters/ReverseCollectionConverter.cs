using ChessGridMVVM.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ChessGridMVVM.Converters
{
    public class ReverseCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Square square)
            {
                return new Square(square.Color, (7 - square.Row), square.Column);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
