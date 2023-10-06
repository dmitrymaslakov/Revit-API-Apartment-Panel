using System;
using System.Windows.Media.Imaging;

namespace DependencyInjectionTest.Core.Services.AnnotationService.AnnotationWriters
{
    public interface IAnnotationWriter : IDisposable
    {
        BitmapSource Save(BitmapSource annotation);
    }
}
