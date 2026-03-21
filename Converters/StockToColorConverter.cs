using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Magazyn_WPF.Converters
{
    public class StockToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int ilosc)
            {
                // Jeœli mniej ni¿ 5 sztuk - kolor czerwony, w przeciwnym razie domyœlny (czarny/ciemny)
                return ilosc < 5 ? Brushes.Red : Brushes.Black;
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}