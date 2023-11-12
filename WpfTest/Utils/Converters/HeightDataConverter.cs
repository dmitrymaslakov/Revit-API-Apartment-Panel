using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfTest.Utils.Converters
{
    public class HeightDataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var typeOfHeight = (string)values[0];
            var defaultHeight = values[1];
            return (typeOfHeight, defaultHeight);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var tuple = (Tuple<object, object>)value;
            return new[] { tuple.Item1, tuple.Item2 };
        }
    }
}
