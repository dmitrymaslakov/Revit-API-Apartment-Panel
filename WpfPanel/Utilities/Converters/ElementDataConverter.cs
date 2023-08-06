using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfPanel.Utilities.Converters
{
    public class ElementDataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)values[0], (string)values[1], (string)values[2]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var tuple = (Tuple<object, object, object>)value;
            return new[] { tuple.Item1, tuple.Item2, tuple.Item3 };
        }
    }
}
