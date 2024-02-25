using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Utility.AnnotationUtility.Interfaces
{
    public interface IAnnotationReader : IDisposable
    {
        //ImageSource Get();
        BitmapImage Get();
    }
}
