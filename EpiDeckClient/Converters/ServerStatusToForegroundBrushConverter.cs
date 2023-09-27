using EpiDeckClient.Domain;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EpiDeckClient.Converters
{
    [ValueConversion(typeof(ServerStatusType), typeof(Brush))]
    public class ServerStatusToForegroundBrushConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var serverStatus = (ServerStatusType)value;

            return serverStatus switch
            {
                ServerStatusType.Disconnected => new SolidColorBrush(Colors.Black),
                ServerStatusType.Connected => new SolidColorBrush(Colors.White),
                ServerStatusType.Error => new SolidColorBrush(Colors.White),
                _ => throw new InvalidEnumArgumentException(nameof(value))
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}