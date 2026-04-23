using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Magazyn_WPF.Converters
{
    // Pokazuje element jeœli string nie jest pusty, chowa jeœli jest pusty
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}