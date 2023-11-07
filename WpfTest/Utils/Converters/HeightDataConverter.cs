using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfTest.Utils.Converters
{
    public class HeightDataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (values[0], values[1]);
            //return values[0];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var tuple = (Tuple<object, object, object>)value;
            return new[] { tuple.Item1, tuple.Item2, tuple.Item3 };
        }
    }
}
