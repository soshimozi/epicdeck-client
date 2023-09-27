using EpiDeckClient.Domain;
using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EpiDeckClient.Converters
{
    [ValueConversion(typeof(ServerStatusType), typeof(Brush))]
    public class ServerStatusToBackgroundBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var serverStatus = (ServerStatusType)value;

            return serverStatus switch
            {
                ServerStatusType.Disconnected => new SolidColorBrush(Colors.Yellow),
                ServerStatusType.Connected => new SolidColorBrush(Colors.Green),
                ServerStatusType.Error => new SolidColorBrush(Colors.Red),
                _ => throw new InvalidEnumArgumentException(nameof(value))
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}