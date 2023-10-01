using System;
using System.Windows.Media.Imaging;

namespace ApartmentPanel.Core.Services.AnnotationService.AnnotationWriters
{
    public interface IAnnotationWriter : IDisposable
    {
        BitmapSource Save(BitmapSource annotation);
    }
}
