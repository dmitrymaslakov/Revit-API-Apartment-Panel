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
            //var customHeight = (string)values[2];
            return (typeOfHeight, defaultHeight);
            //return values[0];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var tuple = (Tuple<object, object, object>)value;
            return new[] { tuple.Item1, tuple.Item2, tuple.Item3 };
        }
    }
}
