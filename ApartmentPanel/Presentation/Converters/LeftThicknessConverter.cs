using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ApartmentPanel.Presentation.Converters
{
    internal class LeftThicknessConverter : IValueConverter //IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness thickness = (Thickness)value;
            return thickness.Left;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string strValue = (string)value;
                Thickness thickness = new Thickness();
                if (double.TryParse(strValue, out double left))
                {
                    //Thickness thickness = new Thickness(left, 0, 0, 0);
                    thickness.Left = left;
                }
                /*var sp = parameter as StackPanel;
                foreach (object tb in sp.Children)
                {
                    /switch (tb.Name)
                    {
                        case "top":
                            if (double.TryParse(tb.Text, out double top))
                                thickness.Top = top;
                            break;
                        case "right":
                            if (double.TryParse(tb.Text, out double right))
                                thickness.Right = right;
                            break;
                        case "bottom":
                            if (double.TryParse(tb.Text, out double bottom))
                                thickness.Bottom = bottom;
                            break;
                    }
                }*/
                return thickness;
            }
            catch 
            {
                return DependencyProperty.UnsetValue; 
            }
        }
    }
}
