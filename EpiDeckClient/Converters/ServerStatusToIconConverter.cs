using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using EpiDeckClient.Domain;
using MaterialDesignThemes.Wpf;

namespace EpiDeckClient.Converters
{
    [ValueConversion(typeof(ServerStatusType), typeof(PackIconKind))]
    public class ServerStatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (ServerStatusType)value;

            switch (status)
            {
                case ServerStatusType.Connected:
                    return PackIconKind.LanConnect;
                case ServerStatusType.Disconnected:
                    return PackIconKind.LanDisconnect;
                case ServerStatusType.Error:
                    return PackIconKind.Error;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}