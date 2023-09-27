using System.Globalization;
using System.Windows.Data;
using System;
using System.Windows.Media;

namespace EpiDeckClient.Converters
{
    public class BoolToLedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isOn)
            {
                return isOn ? Colors.Green : Colors.Gray;
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}