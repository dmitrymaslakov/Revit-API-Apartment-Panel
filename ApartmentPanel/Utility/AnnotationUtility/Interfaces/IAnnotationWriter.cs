using System;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Utility.AnnotationUtility.Interfaces
{
    public interface IAnnotationWriter : IDisposable
    {
        BitmapSource Save(BitmapSource annotation);
    }
}
