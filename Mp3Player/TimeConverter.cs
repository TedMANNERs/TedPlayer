using System;
using System.Globalization;
using System.Windows.Data;

namespace Mp3Player
{
    [ValueConversion(typeof(string), typeof(string))]
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan time = TimeSpan.FromSeconds((int)value);
            return string.Format("{0}:{1:D2}", time.Minutes, time.Seconds);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}