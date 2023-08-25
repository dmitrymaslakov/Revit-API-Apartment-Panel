using System;
using System.Windows.Media.Imaging;

namespace WpfPanel.Domain.Services.AnnotationService.AnnotationWriters
{
    public interface IAnnotationWriter : IDisposable
    {
        BitmapSource Save(BitmapSource annotation);
    }
}